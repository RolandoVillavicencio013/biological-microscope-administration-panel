const { app, BrowserWindow, dialog, shell } = require('electron');
const { spawn } = require('child_process');
const path = require('path');
const http = require('http');
const fs = require('fs');

const BACKEND_URL = 'http://localhost:5000';
const HEALTH_URL = `${BACKEND_URL}/health`;
const HEALTH_TIMEOUT_MS = 90_000;

const PG_PORT = 5432;
const PG_USER = 'postgres';
const PG_PASSWORD = 'postgres';
const PG_DATABASE = 'biomicroscope_admin';
const PG_DATA_DIR = path.join(app.getPath('userData'), 'pgdata');

const resourcesPath = app.isPackaged
  ? process.resourcesPath
  : path.join(__dirname, 'resources');

const backendExePath = path.join(resourcesPath, 'backend', 'UCR.EB.BioMicroscopeAdmin.Backend.Api.exe');

// embedded-postgres ships ESM-only (no CJS build), so it must be loaded via
// dynamic import() from this CommonJS file rather than require().
let pg = null;
async function getPg() {
  if (pg) return pg;
  const { default: EmbeddedPostgres } = await import('embedded-postgres');
  pg = new EmbeddedPostgres({
    databaseDir: PG_DATA_DIR,
    port: PG_PORT,
    user: PG_USER,
    password: PG_PASSWORD,
    persistent: true,
    onLog: (message) => console.log(`[postgres] ${message}`),
    onError: (err) => console.error(`[postgres] ${err}`),
  });
  return pg;
}

let backendProcess = null;
let mainWindow = null;
let shuttingDown = false;

function waitForHealth(url, timeoutMs) {
  const deadline = Date.now() + timeoutMs;
  return new Promise((resolve, reject) => {
    const attempt = () => {
      const req = http.get(url, (res) => {
        res.resume();
        if (res.statusCode === 200) resolve();
        else retry();
      });
      req.on('error', retry);
    };
    const retry = () => {
      if (Date.now() > deadline) {
        reject(new Error(`Timed out waiting for the backend at ${url}`));
        return;
      }
      setTimeout(attempt, 1000);
    };
    attempt();
  });
}

async function startDatabase() {
  const alreadyInitialised = fs.existsSync(path.join(PG_DATA_DIR, 'PG_VERSION'));
  try {
    const db = await getPg();
    if (!alreadyInitialised) {
      await db.initialise();
    }
    await db.start();
    if (!alreadyInitialised) {
      await db.createDatabase(PG_DATABASE);
    }
  } catch (err) {
    throw new Error(`Could not start the embedded database.\n\n${err.message || err}`);
  }
}

async function stopDatabase() {
  if (!pg) return;
  try {
    await pg.stop();
  } catch (err) {
    console.error('Failed to stop the embedded database:', err.message || err);
  }
}

function startBackend() {
  if (!fs.existsSync(backendExePath)) {
    throw new Error(`Backend executable not found at ${backendExePath}`);
  }
  backendProcess = spawn(backendExePath, [], {
    cwd: path.dirname(backendExePath),
    env: {
      ...process.env,
      ASPNETCORE_URLS: BACKEND_URL,
      ASPNETCORE_ENVIRONMENT: 'Production',
      DisableHttpsRedirect: 'true',
    },
  });
  backendProcess.stdout.on('data', (chunk) => console.log(`[backend] ${chunk}`));
  backendProcess.stderr.on('data', (chunk) => console.error(`[backend] ${chunk}`));
  backendProcess.on('exit', (code) => {
    backendProcess = null;
    if (!shuttingDown && code !== 0) {
      dialog.showErrorBox('Backend stopped unexpectedly', `The backend process exited with code ${code}.`);
    }
  });
}

async function createWindow() {
  mainWindow = new BrowserWindow({
    width: 1366,
    height: 860,
    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
      contextIsolation: true,
      nodeIntegration: false,
    },
  });
  mainWindow.on('closed', () => { mainWindow = null; });
  // Links like the Swagger tab use target="_blank"; without this handler
  // Electron silently swallows them instead of opening the OS browser.
  mainWindow.webContents.setWindowOpenHandler(({ url }) => {
    shell.openExternal(url);
    return { action: 'deny' };
  });
  await mainWindow.loadURL(BACKEND_URL);
}

async function shutdown() {
  if (shuttingDown) return;
  shuttingDown = true;
  if (backendProcess) {
    backendProcess.kill();
    backendProcess = null;
  }
  await stopDatabase();
}

app.whenReady().then(async () => {
  try {
    await startDatabase();
    startBackend();
    await waitForHealth(HEALTH_URL, HEALTH_TIMEOUT_MS);
    await createWindow();
  } catch (err) {
    dialog.showErrorBox('Startup failed', err.message || String(err));
    await shutdown();
    app.quit();
  }
});

app.on('window-all-closed', async () => {
  await shutdown();
  app.quit();
});

app.on('before-quit', async (event) => {
  if (!shuttingDown) {
    event.preventDefault();
    await shutdown();
    app.quit();
  }
});

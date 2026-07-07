<#
Builds the Electron desktop distributable:
1. Publishes the Blazor WASM frontend.
2. Publishes Backend.Api as a self-contained win-x64 executable.
3. Merges the WASM output into the backend's wwwroot so Backend.Api serves the SPA.
4. Runs electron-builder to produce the Windows installer (Postgres runs embedded
   via the `embedded-postgres` npm package, no Docker required).
#>

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$buildDir = Join-Path $root 'build'
$frontendPublish = Join-Path $buildDir 'frontend'
$backendPublish = Join-Path $buildDir 'backend'
$electronDir = Join-Path $root 'Electron'
$electronResources = Join-Path $electronDir 'resources'

if (Test-Path $buildDir) { Remove-Item -Recurse -Force $buildDir }
New-Item -ItemType Directory -Path $buildDir | Out-Null

Write-Host "==> Publishing Frontend (Blazor WASM)..." -ForegroundColor Cyan
dotnet publish (Join-Path $root 'Frontend') -c Release -o $frontendPublish

Write-Host "==> Publishing Backend.Api (self-contained win-x64)..." -ForegroundColor Cyan
dotnet publish (Join-Path $root 'Backend.Api') -c Release -r win-x64 --self-contained true `
    -p:PublishSingleFile=false -o $backendPublish

Write-Host "==> Pointing the packaged frontend at the same-origin backend..." -ForegroundColor Cyan
$wasmWwwroot = Join-Path $frontendPublish 'wwwroot'
'{ "ApiBaseUrl": "http://localhost:5000/" }' | Set-Content -Path (Join-Path $wasmWwwroot 'appsettings.json') -Encoding utf8
Remove-Item -Path (Join-Path $wasmWwwroot 'appsettings.json.br'), (Join-Path $wasmWwwroot 'appsettings.json.gz') -ErrorAction SilentlyContinue

Write-Host "==> Merging WASM output into Backend.Api wwwroot..." -ForegroundColor Cyan
$backendWwwroot = Join-Path $backendPublish 'wwwroot'
New-Item -ItemType Directory -Path $backendWwwroot -Force | Out-Null
Copy-Item -Path (Join-Path $wasmWwwroot '*') -Destination $backendWwwroot -Recurse -Force

Write-Host "==> Staging Electron resources..." -ForegroundColor Cyan
$resourcesBackend = Join-Path $electronResources 'backend'
if (Test-Path $resourcesBackend) { Remove-Item -Recurse -Force $resourcesBackend }
New-Item -ItemType Directory -Path $resourcesBackend | Out-Null
Copy-Item -Path (Join-Path $backendPublish '*') -Destination $resourcesBackend -Recurse -Force

Write-Host "==> Building Electron installer..." -ForegroundColor Cyan
Push-Location $electronDir
try {
    if (-not (Test-Path (Join-Path $electronDir 'node_modules'))) {
        npm install
    }
    # We don't sign the installer, so skip electron-builder's signing-identity
    # auto-discovery: it otherwise downloads a winCodeSign bundle containing
    # macOS .dylib symlinks that fail to extract without Windows Developer Mode.
    $env:CSC_IDENTITY_AUTO_DISCOVERY = 'false'
    npm run dist
}
finally {
    Pop-Location
}

Write-Host "==> Done. Installer output: $(Join-Path $electronDir 'dist')" -ForegroundColor Green

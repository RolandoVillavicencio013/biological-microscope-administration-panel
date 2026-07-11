using Microsoft.EntityFrameworkCore;
using UCR.EB.BioMicroscopeAdmin.Backend.DependencyInjection;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence;

// wwwroot está en .gitignore (se genera en el publish), pero UseStaticFiles
// resuelve su WebRootFileProvider al construir el host: si la carpeta no
// existe en ese momento queda fijo a un NullFileProvider y nunca sirve los
// archivos subidos después, aunque LocalFileStorageService los cree en disco.
Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCleanArchitectureServices(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(x => x.AddDefaultPolicy(p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
}

var app = builder.Build();

app.UseGlobalExceptionHandler();

// Swagger queda disponible en todos los entornos: la API es de uso local (sin
// autenticación) tanto en desarrollo como en el empaquetado de escritorio, y la
// pestaña "Documentación API" del frontend enlaza directamente a /swagger.
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

// En desarrollo el frontend consume la API por HTTP; redirigir a HTTPS
// rompe la carga de imágenes en navegadores que no confían en el certificado dev.
// El empaquetado de escritorio (Electron) también corre todo sobre HTTP local
// y define DisableHttpsRedirect para evitar el mismo problema sin certificado.
if (!app.Environment.IsDevelopment() && !builder.Configuration.GetValue<bool>("DisableHttpsRedirect"))
{
    app.UseHttpsRedirection();
}
// Registra los tipos de contenido y la negociación de compresión que Blazor
// WASM necesita para servir archivos como los .dat de ICU, que no tienen un
// content-type reconocido por UseStaticFiles por sí solo.
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapEndpoints();

app.MapGet("/health", async (AppDbContext db) =>
    await db.Database.CanConnectAsync() ? Results.Ok("ok") : Results.StatusCode(503));

// El empaquetado de escritorio sirve la SPA de Blazor WASM directamente desde
// wwwroot; las rutas que no coincidan con la API ni con un archivo estático
// deben caer en index.html para que el enrutamiento del lado del cliente funcione.
app.MapFallbackToFile("index.html");

await MigrateDatabaseWithRetryAsync(app);

await app.RunAsync();

static async Task MigrateDatabaseWithRetryAsync(WebApplication app, int maxAttempts = 30, int delayMs = 2000)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            await db.Database.MigrateAsync();
            return;
        }
        catch (Exception ex) when (attempt < maxAttempts)
        {
            logger.LogWarning(ex, "Database not ready yet (attempt {Attempt}/{MaxAttempts}); retrying in {DelayMs}ms",
                attempt, maxAttempts, delayMs);
            await Task.Delay(delayMs);
        }
    }
}

public partial class Program {}

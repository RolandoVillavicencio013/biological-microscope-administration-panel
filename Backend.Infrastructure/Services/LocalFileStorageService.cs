using System.Linq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Services;

/// <summary>Saves image files to the local wwwroot/uploads folder. Intended for development only.</summary>
internal class LocalFileStorageService : IFileStorageService
{
    private readonly string _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

    public async Task<string> SaveAsync(Stream fileStream, string fileName)
    {
        Directory.CreateDirectory(_uploadsPath);
        var safeName = string.Concat(fileName.Select(c =>
            char.IsLetterOrDigit(c) || c is '.' or '-' or '_' ? c : '_'));
        var uniqueName = $"{Guid.NewGuid()}_{safeName}";
        var fullPath = Path.Combine(_uploadsPath, uniqueName);
        await using var fs = File.Create(fullPath);
        await fileStream.CopyToAsync(fs);
        return $"/uploads/{uniqueName}";
    }

    public Task DeleteAsync(string fileUrl)
    {
        var fileName = Path.GetFileName(fileUrl);
        var fullPath = Path.Combine(_uploadsPath, fileName);
        if (File.Exists(fullPath)) File.Delete(fullPath);
        return Task.CompletedTask;
    }
}

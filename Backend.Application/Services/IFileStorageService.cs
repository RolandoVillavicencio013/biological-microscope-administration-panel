namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;

/// <summary>Contract for saving and deleting image files on the underlying storage.</summary>
public interface IFileStorageService
{
    /// <summary>Saves a file stream to storage and returns the relative URL to access it.</summary>
    Task<string> SaveAsync(Stream fileStream, string fileName);

    /// <summary>Deletes the file at the given relative URL.</summary>
    Task DeleteAsync(string fileUrl);
}

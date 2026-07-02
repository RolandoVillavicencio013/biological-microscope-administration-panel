using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Services;
using Xunit;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Tests.Services;

/// <summary>Unit tests for <see cref="LocalFileStorageService"/>.</summary>
public class LocalFileStorageServiceTests : IDisposable
{
    private readonly LocalFileStorageService _service;
    private readonly string _testUploadsPath;

    public LocalFileStorageServiceTests()
    {
        _service = new LocalFileStorageService();
        // Crear directorio temporal para tests
        _testUploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(_testUploadsPath))
            Directory.CreateDirectory(_testUploadsPath);
    }

    [Fact]
    public async Task SaveAsync_WithValidStream_CreatesFileAndReturnsPath()
    {
        // Arrange
        var fileName = "test-image.jpg";
        var fileContent = "fake image content";
        await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));

        // Act
        var result = await _service.SaveAsync(stream, fileName);

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("/uploads/", result);
        Assert.EndsWith("_test-image.jpg", result);
        Assert.True(Guid.TryParse(result.Split('_')[1].Split('.')[0], out _), "Filename should contain GUID");

        // Verify file exists
        var fileNameOnly = Path.GetFileName(result);
        var fullPath = Path.Combine(_testUploadsPath, fileNameOnly);
        Assert.True(File.Exists(fullPath));

        // Cleanup
        File.Delete(fullPath);
    }

    [Fact]
    public async Task SaveAsync_WithMultipleFiles_CreatesUniqueNames()
    {
        // Arrange
        var fileName = "image.jpg";
        var fileContent = "content";

        // Act
        var result1 = await _service.SaveAsync(
            new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent)),
            fileName);
        var result2 = await _service.SaveAsync(
            new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent)),
            fileName);

        // Assert
        Assert.NotEqual(result1, result2);

        // Cleanup
        foreach (var filePath in new[] { result1, result2 })
        {
            var fileNameOnly = Path.GetFileName(filePath);
            var fullPath = Path.Combine(_testUploadsPath, fileNameOnly);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }

    [Fact]
    public async Task DeleteAsync_WithExistingFile_DeletesFile()
    {
        // Arrange
        var fileName = "test-delete.jpg";
        var fileContent = "content to delete";
        await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));

        var filePath = await _service.SaveAsync(stream, fileName);
        var fileNameOnly = Path.GetFileName(filePath);
        var fullPath = Path.Combine(_testUploadsPath, fileNameOnly);
        Assert.True(File.Exists(fullPath));

        // Act
        await _service.DeleteAsync(filePath);

        // Assert
        Assert.False(File.Exists(fullPath));
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentFile_DoesNotThrow()
    {
        // Arrange
        var fileUrl = "/uploads/nonexistent-guid_file.jpg";

        // Act & Assert - Should not throw
        await _service.DeleteAsync(fileUrl);
    }

    [Fact]
    public async Task SaveAsync_WithEmptyStream_StoresEmptyFile()
    {
        // Arrange
        var fileName = "empty.jpg";
        await using var emptyStream = new MemoryStream();

        // Act
        var result = await _service.SaveAsync(emptyStream, fileName);

        // Assert
        Assert.NotNull(result);
        var fileNameOnly = Path.GetFileName(result);
        var fullPath = Path.Combine(_testUploadsPath, fileNameOnly);
        var fileInfo = new FileInfo(fullPath);
        Assert.Equal(0, fileInfo.Length);

        // Cleanup
        File.Delete(fullPath);
    }

    [Fact]
    public async Task SaveAsync_PreservesFileName()
    {
        // Arrange
        var fileName = "my-special-image.jpg";
        await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("content"));

        // Act
        var result = await _service.SaveAsync(stream, fileName);

        // Assert
        Assert.EndsWith(fileName, result);
    }

    public void Dispose()
    {
        // Cleanup test files
        if (Directory.Exists(_testUploadsPath))
        {
            try
            {
                var files = Directory.GetFiles(_testUploadsPath);
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }
}

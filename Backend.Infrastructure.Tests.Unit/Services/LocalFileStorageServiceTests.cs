using System.Text;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Services;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Tests.Unit.Services;

public class LocalFileStorageServiceTests
{
    private const string FileContent = "sample image bytes";

    private static Stream ContentStream() => new MemoryStream(Encoding.UTF8.GetBytes(FileContent));

    private static string ToFullPath(string fileUrl) =>
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", Path.GetFileName(fileUrl));

    [Fact]
    public async Task SaveAsync_ValidStream_ReturnsUrlWithUploadsPrefix()
    {
        // Arrange
        var service = new LocalFileStorageService();

        // Act
        var fileUrl = await service.SaveAsync(ContentStream(), "sample.jpg");
        File.Delete(ToFullPath(fileUrl));

        // Assert
        Assert.StartsWith("/uploads/", fileUrl);
    }

    [Fact]
    public async Task SaveAsync_FileNameWithUnsafeCharacters_SanitizesThemToUnderscore()
    {
        // Arrange
        var service = new LocalFileStorageService();

        // Act
        var fileUrl = await service.SaveAsync(ContentStream(), "a b?c.jpg");
        File.Delete(ToFullPath(fileUrl));

        // Assert
        Assert.EndsWith("a_b_c.jpg", fileUrl);
    }

    [Fact]
    public async Task SaveAsync_ValidStream_WritesFileWithMatchingContent()
    {
        // Arrange
        var service = new LocalFileStorageService();

        // Act
        var fileUrl = await service.SaveAsync(ContentStream(), "sample.jpg");
        var persistedContent = await File.ReadAllTextAsync(ToFullPath(fileUrl));
        File.Delete(ToFullPath(fileUrl));

        // Assert
        Assert.Equal(FileContent, persistedContent);
    }

    [Fact]
    public async Task SaveAsync_CalledTwiceWithSameFileName_ProducesDifferentUrls()
    {
        // Arrange
        var service = new LocalFileStorageService();

        // Act
        var firstUrl = await service.SaveAsync(ContentStream(), "sample.jpg");
        var secondUrl = await service.SaveAsync(ContentStream(), "sample.jpg");
        File.Delete(ToFullPath(firstUrl));
        File.Delete(ToFullPath(secondUrl));

        // Assert
        Assert.NotEqual(firstUrl, secondUrl);
    }

    [Fact]
    public async Task DeleteAsync_ExistingFile_RemovesFile()
    {
        // Arrange
        var service = new LocalFileStorageService();
        var fileUrl = await service.SaveAsync(ContentStream(), "sample.jpg");

        // Act
        await service.DeleteAsync(fileUrl);

        // Assert
        Assert.False(File.Exists(ToFullPath(fileUrl)));
    }

    [Fact]
    public async Task DeleteAsync_NonExistingFile_DoesNotThrow()
    {
        // Arrange
        var service = new LocalFileStorageService();

        // Act & Assert
        await service.DeleteAsync("/uploads/does-not-exist.jpg");
    }
}

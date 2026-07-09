using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services.Implementations;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Tests.Unit.Services;

public class ImageServiceTests
{
    private const int ExistingId = 21;
    private const int ParentSlideId = 4;

    private static Image NewImage() => new(
        ImageUrl.Create("/uploads/slides/4/sample.jpg"),
        ImageFileName.Create("sample.jpg"),
        ImageOrder.Create(1),
        ParentSlideId);

    [Fact]
    public async Task CreateAsync_ValidImage_ReturnsRepositoryResult()
    {
        // Arrange
        var image = NewImage();
        var created = new Image(ExistingId, image.FileUrl, image.FileName, image.Order, image.UploadedAt, ParentSlideId);
        var repository = new Mock<IImageRepository>();
        repository.Setup(r => r.CreateAsync(image)).ReturnsAsync(created);
        var service = new ImageService(repository.Object);

        // Act
        var result = await service.CreateAsync(image);

        // Assert
        Assert.Same(created, result);
    }

    [Fact]
    public async Task FindByIdAsync_ExistingId_ReturnsImageFromRepository()
    {
        // Arrange
        var image = NewImage();
        var repository = new Mock<IImageRepository>();
        repository.Setup(r => r.ReadAsync(ExistingId)).ReturnsAsync(image);
        var service = new ImageService(repository.Object);

        // Act
        var result = await service.FindByIdAsync(ExistingId);

        // Assert
        Assert.Same(image, result);
    }

    [Fact]
    public async Task FindByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var repository = new Mock<IImageRepository>();
        repository.Setup(r => r.ReadAsync(ExistingId)).ReturnsAsync((Image?)null);
        var service = new ImageService(repository.Object);

        // Act
        var result = await service.FindByIdAsync(ExistingId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindBySlideAsync_DelegatesToRepositoryListBySlideAsync()
    {
        // Arrange
        var images = new List<Image> { NewImage() };
        var repository = new Mock<IImageRepository>();
        repository.Setup(r => r.ListBySlideAsync(ParentSlideId)).ReturnsAsync(images);
        var service = new ImageService(repository.Object);

        // Act
        var result = await service.FindBySlideAsync(ParentSlideId);

        // Assert
        Assert.Same(images, result);
    }

    [Fact]
    public async Task UpdateAsync_ValidImage_ReturnsRepositoryResult()
    {
        // Arrange
        var image = NewImage();
        var repository = new Mock<IImageRepository>();
        repository.Setup(r => r.UpdateAsync(image)).ReturnsAsync(image);
        var service = new ImageService(repository.Object);

        // Act
        var result = await service.UpdateAsync(image);

        // Assert
        Assert.Same(image, result);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_CallsRepositoryDeleteAsyncOnce()
    {
        // Arrange
        var repository = new Mock<IImageRepository>();
        var service = new ImageService(repository.Object);

        // Act
        await service.DeleteAsync(ExistingId);

        // Assert
        repository.Verify(r => r.DeleteAsync(ExistingId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var image = NewImage();
        var repository = new Mock<IImageRepository>();
        repository.Setup(r => r.CreateAsync(image)).ThrowsAsync(new InvalidOperationException("db failure"));
        var service = new ImageService(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(image));
    }
}

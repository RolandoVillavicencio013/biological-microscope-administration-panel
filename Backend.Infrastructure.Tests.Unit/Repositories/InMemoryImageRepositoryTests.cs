using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Tests.Unit.Repositories;

public class InMemoryImageRepositoryTests
{
    private const int ParentSlideId = 4;
    private const int OtherSlideId = 5;

    private static Image NewImage(int order, int slideId = ParentSlideId) => new(
        ImageUrl.Create($"/img{order}.jpg"), ImageFileName.Create($"img{order}.jpg"),
        ImageOrder.Create(order), slideId);

    [Fact]
    public async Task CreateAsync_TwoImages_AssignsSequentialIds()
    {
        // Arrange
        var repository = new InMemoryImageRepository();

        // Act
        var first = await repository.CreateAsync(NewImage(1));
        var second = await repository.CreateAsync(NewImage(2));

        // Assert
        Assert.Equal(1, first.Id);
        Assert.Equal(2, second.Id);
    }

    [Fact]
    public async Task ReadAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var repository = new InMemoryImageRepository();

        // Act
        var result = await repository.ReadAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ListBySlideAsync_ReturnsOnlyImagesForRequestedSlide()
    {
        // Arrange
        var repository = new InMemoryImageRepository();
        await repository.CreateAsync(NewImage(1, ParentSlideId));
        await repository.CreateAsync(NewImage(1, OtherSlideId));

        // Act
        var result = await repository.ListBySlideAsync(ParentSlideId);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task ListBySlideAsync_ReturnsImagesOrderedByOrderValue()
    {
        // Arrange
        var repository = new InMemoryImageRepository();
        await repository.CreateAsync(NewImage(3));
        await repository.CreateAsync(NewImage(1));
        await repository.CreateAsync(NewImage(2));

        // Act
        var result = await repository.ListBySlideAsync(ParentSlideId);

        // Assert
        Assert.Equal([1, 2, 3], result.Select(i => i.Order.Value));
    }

    [Fact]
    public async Task UpdateAsync_ExistingImage_ReplacesStoredOrder()
    {
        // Arrange
        var repository = new InMemoryImageRepository();
        var created = await repository.CreateAsync(NewImage(1));
        var updated = new Image(created.Id, created.FileUrl, created.FileName,
            ImageOrder.Create(6), created.UploadedAt, ParentSlideId);

        // Act
        await repository.UpdateAsync(updated);
        var result = await repository.ReadAsync(created.Id);

        // Assert
        Assert.Equal(6, result!.Order.Value);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var repository = new InMemoryImageRepository();
        var image = new Image(1, ImageUrl.Create("/img.jpg"), ImageFileName.Create("img.jpg"),
            ImageOrder.Create(1), DateTime.UtcNow, ParentSlideId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.UpdateAsync(image));
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_RemovesImage()
    {
        // Arrange
        var repository = new InMemoryImageRepository();
        var created = await repository.CreateAsync(NewImage(1));

        // Act
        await repository.DeleteAsync(created.Id);
        var result = await repository.ReadAsync(created.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var repository = new InMemoryImageRepository();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.DeleteAsync(1));
    }
}

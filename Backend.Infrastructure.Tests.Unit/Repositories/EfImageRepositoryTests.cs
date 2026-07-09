using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Tests.Unit.Repositories;

public class EfImageRepositoryTests
{
    private static Image NewImage(int slideId, int order) => new(
        ImageUrl.Create($"/img{order}.jpg"), ImageFileName.Create($"img{order}.jpg"),
        ImageOrder.Create(order), slideId);

    private static async Task<int> SeedSlideAsync(AppDbContext context)
    {
        var group = new Group(GroupName.Create("Bacteria"));
        context.Groups.Add(group);
        await context.SaveChangesAsync();

        var box = new Box(BoxNumber.Create(7), BoxName.Create("Cyanobacteria"), group.Id);
        context.Boxes.Add(box);
        await context.SaveChangesAsync();

        var slide = new Slide(SlideCode.Create("1TB07"), SlideName.Create("Gloecapsa"), box.Id);
        context.Slides.Add(slide);
        await context.SaveChangesAsync();
        return slide.Id;
    }

    [Fact]
    public async Task CreateAsync_ValidImage_AssignsGeneratedId()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var slideId = await SeedSlideAsync(database.Context);
        var repository = new EfImageRepository(database.Context);

        // Act
        var created = await repository.CreateAsync(NewImage(slideId, 1));

        // Assert
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task ReadAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var repository = new EfImageRepository(database.Context);

        // Act
        var result = await repository.ReadAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ListBySlideAsync_ReturnsOnlyImagesForRequestedSlide()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var targetSlideId = await SeedSlideAsync(database.Context);
        var otherSlideId = await SeedSlideAsync(database.Context);
        var repository = new EfImageRepository(database.Context);
        await repository.CreateAsync(NewImage(targetSlideId, 1));
        await repository.CreateAsync(NewImage(otherSlideId, 1));

        // Act
        var result = await repository.ListBySlideAsync(targetSlideId);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task ListBySlideAsync_ReturnsImagesOrderedByOrder()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var slideId = await SeedSlideAsync(database.Context);
        var repository = new EfImageRepository(database.Context);
        await repository.CreateAsync(NewImage(slideId, 3));
        await repository.CreateAsync(NewImage(slideId, 1));
        await repository.CreateAsync(NewImage(slideId, 2));

        // Act
        var result = await repository.ListBySlideAsync(slideId);

        // Assert
        Assert.Equal([1, 2, 3], result.Select(i => i.Order.Value));
    }

    [Fact]
    public async Task UpdateAsync_ExistingImage_PersistsNewOrder()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var slideId = await SeedSlideAsync(database.Context);
        var created = await new EfImageRepository(database.Context).CreateAsync(NewImage(slideId, 1));
        var updated = new Image(created.Id, created.FileUrl, created.FileName,
            ImageOrder.Create(6), created.UploadedAt, slideId);

        // Act
        using var updateContext = database.CreateContext();
        await new EfImageRepository(updateContext).UpdateAsync(updated);

        // Assert
        var result = await new EfImageRepository(database.Context).ReadAsync(created.Id);
        Assert.Equal(6, result!.Order.Value);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var slideId = await SeedSlideAsync(database.Context);
        var repository = new EfImageRepository(database.Context);
        var image = new Image(1, ImageUrl.Create("/img.jpg"), ImageFileName.Create("img.jpg"),
            ImageOrder.Create(1), DateTime.UtcNow, slideId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.UpdateAsync(image));
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_RemovesImage()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var slideId = await SeedSlideAsync(database.Context);
        var repository = new EfImageRepository(database.Context);
        var created = await repository.CreateAsync(NewImage(slideId, 1));

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
        using var database = new SqliteTestDatabase();
        var repository = new EfImageRepository(database.Context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.DeleteAsync(1));
    }
}

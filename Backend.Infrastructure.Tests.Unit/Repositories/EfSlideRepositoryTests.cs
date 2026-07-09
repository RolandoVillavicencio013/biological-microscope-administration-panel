using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Tests.Unit.Repositories;

public class EfSlideRepositoryTests
{
    private const int ValidPage = 1;
    private const int ValidPageSize = 20;

    private static Slide NewSlide(int boxId, string code = "1TB07", string name = "Gloecapsa") =>
        new(SlideCode.Create(code), SlideName.Create(name), boxId);

    private static async Task<int> SeedBoxAsync(AppDbContext context)
    {
        var group = new Group(GroupName.Create("Bacteria"));
        context.Groups.Add(group);
        await context.SaveChangesAsync();

        var box = new Box(BoxNumber.Create(7), BoxName.Create("Cyanobacteria"), group.Id);
        context.Boxes.Add(box);
        await context.SaveChangesAsync();
        return box.Id;
    }

    [Fact]
    public async Task CreateAsync_ValidSlide_AssignsGeneratedId()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var boxId = await SeedBoxAsync(database.Context);
        var repository = new EfSlideRepository(database.Context);

        // Act
        var created = await repository.CreateAsync(NewSlide(boxId));

        // Assert
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task ReadAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var repository = new EfSlideRepository(database.Context);

        // Act
        var result = await repository.ReadAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ReadAsync_SlideWithImages_IncludesImages()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var boxId = await SeedBoxAsync(database.Context);
        var created = await new EfSlideRepository(database.Context).CreateAsync(NewSlide(boxId));
        database.Context.Images.Add(new Image(
            ImageUrl.Create("/img.jpg"), ImageFileName.Create("img.jpg"), ImageOrder.Create(1), created.Id));
        await database.Context.SaveChangesAsync();

        // Act
        using var readContext = database.CreateContext();
        var result = await new EfSlideRepository(readContext).ReadAsync(created.Id);

        // Assert
        Assert.Single(result!.Images);
    }

    [Fact]
    public async Task ListByBoxAsync_ReturnsOnlySlidesForRequestedBox()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var targetBoxId = await SeedBoxAsync(database.Context);
        var otherBoxId = await SeedBoxAsync(database.Context);
        var repository = new EfSlideRepository(database.Context);
        await repository.CreateAsync(NewSlide(targetBoxId));
        await repository.CreateAsync(NewSlide(otherBoxId, "2TB08", "Anabaena"));

        // Act
        var result = await repository.ListByBoxAsync(targetBoxId);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_MatchesCodeOrNameCaseInsensitively()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var boxId = await SeedBoxAsync(database.Context);
        var repository = new EfSlideRepository(database.Context);
        await repository.CreateAsync(NewSlide(boxId, "1TB07", "Gloecapsa"));

        // Act
        var (items, totalCount) = await repository.SearchAsync("gloe", null, ValidPage, ValidPageSize);

        // Assert
        Assert.Equal(1, totalCount);
        Assert.Single(items);
    }

    [Fact]
    public async Task SearchAsync_WithBoxIdFilter_ExcludesSlidesFromOtherBoxes()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var targetBoxId = await SeedBoxAsync(database.Context);
        var otherBoxId = await SeedBoxAsync(database.Context);
        var repository = new EfSlideRepository(database.Context);
        await repository.CreateAsync(NewSlide(targetBoxId));
        await repository.CreateAsync(NewSlide(otherBoxId, "2TB08", "Anabaena"));

        // Act
        var (items, totalCount) = await repository.SearchAsync(null, targetBoxId, ValidPage, ValidPageSize);

        // Assert
        Assert.Equal(1, totalCount);
        Assert.Single(items);
    }

    [Fact]
    public async Task SearchAsync_TotalCountReflectsAllMatchesIgnoringPaging()
    {
        // Arrange
        const int pageSize = 1;
        using var database = new SqliteTestDatabase();
        var boxId = await SeedBoxAsync(database.Context);
        var repository = new EfSlideRepository(database.Context);
        await repository.CreateAsync(NewSlide(boxId, "1TB07", "Gloecapsa"));
        await repository.CreateAsync(NewSlide(boxId, "2TB08", "Anabaena"));

        // Act
        var (items, totalCount) = await repository.SearchAsync(null, null, ValidPage, pageSize);

        // Assert
        Assert.Single(items);
        Assert.Equal(2, totalCount);
    }

    [Fact]
    public async Task UpdateAsync_ExistingSlide_PersistsNewDescription()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var boxId = await SeedBoxAsync(database.Context);
        var created = await new EfSlideRepository(database.Context).CreateAsync(NewSlide(boxId));
        var updated = new Slide(created.Id, created.Code, created.Name, boxId,
            Text.Create("New description"));

        // Act
        using var updateContext = database.CreateContext();
        await new EfSlideRepository(updateContext).UpdateAsync(updated);

        // Assert
        var result = await new EfSlideRepository(database.Context).ReadAsync(created.Id);
        Assert.Equal("New description", result!.Description?.Value);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var boxId = await SeedBoxAsync(database.Context);
        var repository = new EfSlideRepository(database.Context);
        var slide = new Slide(1, SlideCode.Create("1TB07"), SlideName.Create("Gloecapsa"), boxId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.UpdateAsync(slide));
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_RemovesSlide()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var boxId = await SeedBoxAsync(database.Context);
        var repository = new EfSlideRepository(database.Context);
        var created = await repository.CreateAsync(NewSlide(boxId));

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
        var repository = new EfSlideRepository(database.Context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.DeleteAsync(1));
    }
}

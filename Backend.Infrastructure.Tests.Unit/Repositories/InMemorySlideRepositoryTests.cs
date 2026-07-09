using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Tests.Unit.Repositories;

public class InMemorySlideRepositoryTests
{
    private const int ParentBoxId = 11;
    private const int OtherBoxId = 12;
    private const int ValidPage = 1;
    private const int ValidPageSize = 20;

    private static Slide NewSlide(string code = "1TB07", string name = "Gloecapsa", int boxId = ParentBoxId) =>
        new(SlideCode.Create(code), SlideName.Create(name), boxId);

    [Fact]
    public async Task CreateAsync_TwoSlides_AssignsSequentialIds()
    {
        // Arrange
        var repository = new InMemorySlideRepository();

        // Act
        var first = await repository.CreateAsync(NewSlide());
        var second = await repository.CreateAsync(NewSlide("2TB08", "Anabaena"));

        // Assert
        Assert.Equal(1, first.Id);
        Assert.Equal(2, second.Id);
    }

    [Fact]
    public async Task ReadAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var repository = new InMemorySlideRepository();

        // Act
        var result = await repository.ReadAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ListByBoxAsync_ReturnsOnlySlidesForRequestedBox()
    {
        // Arrange
        var repository = new InMemorySlideRepository();
        await repository.CreateAsync(NewSlide(boxId: ParentBoxId));
        await repository.CreateAsync(NewSlide("2TB08", "Anabaena", OtherBoxId));

        // Act
        var result = await repository.ListByBoxAsync(ParentBoxId);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_MatchesCodeCaseInsensitively()
    {
        // Arrange
        var repository = new InMemorySlideRepository();
        await repository.CreateAsync(NewSlide("1TB07", "Gloecapsa"));

        // Act
        var (items, totalCount) = await repository.SearchAsync("tb07", null, ValidPage, ValidPageSize);

        // Assert
        Assert.Equal(1, totalCount);
        Assert.Single(items);
    }

    [Fact]
    public async Task SearchAsync_MatchesNameCaseInsensitively()
    {
        // Arrange
        var repository = new InMemorySlideRepository();
        await repository.CreateAsync(NewSlide("1TB07", "Gloecapsa"));

        // Act
        var (items, totalCount) = await repository.SearchAsync("gloe", null, ValidPage, ValidPageSize);

        // Assert
        Assert.Equal(1, totalCount);
        Assert.Single(items);
    }

    [Fact]
    public async Task SearchAsync_NoMatch_ReturnsEmptyWithZeroTotalCount()
    {
        // Arrange
        var repository = new InMemorySlideRepository();
        await repository.CreateAsync(NewSlide("1TB07", "Gloecapsa"));

        // Act
        var (items, totalCount) = await repository.SearchAsync("nonexistent", null, ValidPage, ValidPageSize);

        // Assert
        Assert.Empty(items);
        Assert.Equal(0, totalCount);
    }

    [Fact]
    public async Task SearchAsync_WithBoxIdFilter_ExcludesSlidesFromOtherBoxes()
    {
        // Arrange
        var repository = new InMemorySlideRepository();
        await repository.CreateAsync(NewSlide(boxId: ParentBoxId));
        await repository.CreateAsync(NewSlide("2TB08", "Anabaena", OtherBoxId));

        // Act
        var (items, totalCount) = await repository.SearchAsync(null, ParentBoxId, ValidPage, ValidPageSize);

        // Assert
        Assert.Equal(1, totalCount);
        Assert.Single(items);
    }

    [Fact]
    public async Task SearchAsync_TotalCountReflectsAllMatchesIgnoringPaging()
    {
        // Arrange
        const int pageSize = 1;
        var repository = new InMemorySlideRepository();
        await repository.CreateAsync(NewSlide("1TB07", "Gloecapsa"));
        await repository.CreateAsync(NewSlide("2TB08", "Anabaena"));

        // Act
        var (items, totalCount) = await repository.SearchAsync(null, null, ValidPage, pageSize);

        // Assert
        Assert.Single(items);
        Assert.Equal(2, totalCount);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var repository = new InMemorySlideRepository();
        var slide = new Slide(1, SlideCode.Create("1TB07"), SlideName.Create("Gloecapsa"), ParentBoxId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.UpdateAsync(slide));
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_RemovesSlide()
    {
        // Arrange
        var repository = new InMemorySlideRepository();
        var created = await repository.CreateAsync(NewSlide());

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
        var repository = new InMemorySlideRepository();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.DeleteAsync(1));
    }
}

using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services.Implementations;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Tests.Unit.Services;

public class SlideServiceTests
{
    private const int ExistingId = 3;
    private const int ParentBoxId = 11;
    private const string SearchText = "Gloe";
    private const int Page = 1;
    private const int PageSize = 20;

    private static Slide NewSlide() => new(SlideCode.Create("1TB07"), SlideName.Create("Gloecapsa"), ParentBoxId);

    [Fact]
    public async Task CreateAsync_ValidSlide_ReturnsRepositoryResult()
    {
        // Arrange
        var slide = NewSlide();
        var created = new Slide(ExistingId, slide.Code, slide.Name, ParentBoxId);
        var repository = new Mock<ISlideRepository>();
        repository.Setup(r => r.CreateAsync(slide)).ReturnsAsync(created);
        var service = new SlideService(repository.Object);

        // Act
        var result = await service.CreateAsync(slide);

        // Assert
        Assert.Same(created, result);
    }

    [Fact]
    public async Task FindByIdAsync_ExistingId_ReturnsSlideFromRepository()
    {
        // Arrange
        var slide = NewSlide();
        var repository = new Mock<ISlideRepository>();
        repository.Setup(r => r.ReadAsync(ExistingId)).ReturnsAsync(slide);
        var service = new SlideService(repository.Object);

        // Act
        var result = await service.FindByIdAsync(ExistingId);

        // Assert
        Assert.Same(slide, result);
    }

    [Fact]
    public async Task FindByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var repository = new Mock<ISlideRepository>();
        repository.Setup(r => r.ReadAsync(ExistingId)).ReturnsAsync((Slide?)null);
        var service = new SlideService(repository.Object);

        // Act
        var result = await service.FindByIdAsync(ExistingId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindByBoxAsync_DelegatesToRepositoryListByBoxAsync()
    {
        // Arrange
        var slides = new List<Slide> { NewSlide() };
        var repository = new Mock<ISlideRepository>();
        repository.Setup(r => r.ListByBoxAsync(ParentBoxId)).ReturnsAsync(slides);
        var service = new SlideService(repository.Object);

        // Act
        var result = await service.FindByBoxAsync(ParentBoxId);

        // Assert
        Assert.Same(slides, result);
    }

    [Fact]
    public async Task SearchAsync_DelegatesToRepositorySearchAsyncAndReturnsItemsAndTotalCount()
    {
        // Arrange
        var slides = new List<Slide> { NewSlide() };
        const int totalCount = 1;
        var repository = new Mock<ISlideRepository>();
        repository.Setup(r => r.SearchAsync(SearchText, ParentBoxId, Page, PageSize))
            .ReturnsAsync((slides, totalCount));
        var service = new SlideService(repository.Object);

        // Act
        var (items, resultTotalCount) = await service.SearchAsync(SearchText, ParentBoxId, Page, PageSize);

        // Assert
        Assert.Same(slides, items);
        Assert.Equal(totalCount, resultTotalCount);
    }

    [Fact]
    public async Task UpdateAsync_ValidSlide_ReturnsRepositoryResult()
    {
        // Arrange
        var slide = NewSlide();
        var repository = new Mock<ISlideRepository>();
        repository.Setup(r => r.UpdateAsync(slide)).ReturnsAsync(slide);
        var service = new SlideService(repository.Object);

        // Act
        var result = await service.UpdateAsync(slide);

        // Assert
        Assert.Same(slide, result);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_CallsRepositoryDeleteAsyncOnce()
    {
        // Arrange
        var repository = new Mock<ISlideRepository>();
        var service = new SlideService(repository.Object);

        // Act
        await service.DeleteAsync(ExistingId);

        // Assert
        repository.Verify(r => r.DeleteAsync(ExistingId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var slide = NewSlide();
        var repository = new Mock<ISlideRepository>();
        repository.Setup(r => r.CreateAsync(slide)).ThrowsAsync(new InvalidOperationException("db failure"));
        var service = new SlideService(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(slide));
    }
}

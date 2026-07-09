using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Slides;

public class DeleteSlideHandlerTests
{
    private const int ExistingId = 3;
    private const int ParentBoxId = 11;

    private static Slide ExistingSlide() =>
        new(ExistingId, SlideCode.Create("1TB07"), SlideName.Create("Gloecapsa"), ParentBoxId);

    [Fact]
    public async Task HandleAsync_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var service = new Mock<ISlideService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingSlide());

        // Act
        var result = await DeleteSlideHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Fact]
    public async Task HandleAsync_ExistingId_CallsServiceDeleteAsyncOnce()
    {
        // Arrange
        var service = new Mock<ISlideService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingSlide());

        // Act
        await DeleteSlideHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        service.Verify(s => s.DeleteAsync(ExistingId), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var service = new Mock<ISlideService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync((Slide?)null);

        // Act
        var result = await DeleteSlideHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }
}

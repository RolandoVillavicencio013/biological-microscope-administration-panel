using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Slides;

public class GetSlideHandlerTests
{
    private const int ExistingId = 3;
    private const int ParentBoxId = 11;

    [Fact]
    public async Task HandleAsync_ExistingId_ReturnsOkWithMappedDetailResponseIncludingImages()
    {
        // Arrange
        var slide = new Slide(ExistingId, SlideCode.Create("1TB07"), SlideName.Create("Gloecapsa"), ParentBoxId);
        slide.Images.Add(new Image(ImageUrl.Create("/img.jpg"), ImageFileName.Create("img.jpg"), ImageOrder.Create(1), ExistingId));
        var service = new Mock<ISlideService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(slide);

        // Act
        var result = await GetSlideHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        var okResult = Assert.IsType<Ok<SlideDetailResponse>>(result.Result);
        Assert.Single(okResult.Value!.Images);
    }

    [Fact]
    public async Task HandleAsync_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var service = new Mock<ISlideService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync((Slide?)null);

        // Act
        var result = await GetSlideHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }
}

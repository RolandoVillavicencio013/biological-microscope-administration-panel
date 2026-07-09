using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Slides;

public class UpdateSlideHandlerTests
{
    private const int ExistingId = 3;
    private const int ParentBoxId = 11;
    private const string ValidDescription = "Updated description";

    private static Slide ExistingSlide() =>
        new(ExistingId, SlideCode.Create("1TB07"), SlideName.Create("Gloecapsa"), ParentBoxId,
            Text.Create("Old description"));

    [Fact]
    public async Task HandleAsync_ExistingIdAndValidRequest_ReturnsOkWithUpdatedResponse()
    {
        // Arrange
        var request = new UpdateSlideRequest(ValidDescription, null);
        var service = new Mock<ISlideService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingSlide());
        service.Setup(s => s.UpdateAsync(It.IsAny<Slide>())).ReturnsAsync((Slide slide) => slide);

        // Act
        var result = await UpdateSlideHandler.HandleAsync(ExistingId, request, service.Object);

        // Assert
        var okResult = Assert.IsType<Ok<SlideResponse>>(result.Result);
        Assert.Equal(ValidDescription, okResult.Value!.Description);
    }

    [Fact]
    public async Task HandleAsync_NullDescriptionInRequest_ClearsExistingDescription()
    {
        // Arrange
        var request = new UpdateSlideRequest(null, null);
        var service = new Mock<ISlideService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingSlide());
        service.Setup(s => s.UpdateAsync(It.IsAny<Slide>())).ReturnsAsync((Slide slide) => slide);

        // Act
        var result = await UpdateSlideHandler.HandleAsync(ExistingId, request, service.Object);

        // Assert
        var okResult = Assert.IsType<Ok<SlideResponse>>(result.Result);
        Assert.Null(okResult.Value!.Description);
    }

    [Fact]
    public async Task HandleAsync_NonExistingIdWithInvalidRequest_ReturnsNotFoundNotBadRequest()
    {
        // Arrange
        var request = new UpdateSlideRequest("   ", null);
        var service = new Mock<ISlideService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync((Slide?)null);

        // Act
        var result = await UpdateSlideHandler.HandleAsync(ExistingId, request, service.Object);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }

    [Fact]
    public async Task HandleAsync_ExistingIdWithWhitespaceOnlyDescription_ReturnsBadRequestWithValidationError()
    {
        // Arrange
        var request = new UpdateSlideRequest("   ", null);
        var service = new Mock<ISlideService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingSlide());

        // Act
        var result = await UpdateSlideHandler.HandleAsync(ExistingId, request, service.Object);

        // Assert
        Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
    }
}

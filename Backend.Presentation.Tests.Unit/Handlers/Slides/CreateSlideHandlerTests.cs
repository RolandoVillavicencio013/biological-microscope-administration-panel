using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Slides;

public class CreateSlideHandlerTests
{
    private const string ValidCode = "1TB07";
    private const string ValidName = "Gloecapsa";
    private const int ParentBoxId = 11;
    private const int NewId = 3;

    [Fact]
    public async Task HandleAsync_ValidRequest_ReturnsCreatedWithMappedResponse()
    {
        // Arrange
        var request = new CreateSlideRequest(ValidCode, ValidName, ParentBoxId, null, null);
        var created = new Slide(NewId, SlideCode.Create(ValidCode), SlideName.Create(ValidName), ParentBoxId);
        var service = new Mock<ISlideService>();
        service.Setup(s => s.CreateAsync(It.IsAny<Slide>())).ReturnsAsync(created);

        // Act
        var result = await CreateSlideHandler.HandleAsync(request, service.Object);

        // Assert
        var createdResult = Assert.IsType<Created<SlideResponse>>(result.Result);
        Assert.Equal(NewId, createdResult.Value!.Id);
    }

    [Fact]
    public async Task HandleAsync_InvalidRequest_ReturnsBadRequestWithValidationErrors()
    {
        // Arrange
        var request = new CreateSlideRequest("", "", ParentBoxId, null, null);
        var service = new Mock<ISlideService>();

        // Act
        var result = await CreateSlideHandler.HandleAsync(request, service.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        Assert.Equal(2, badRequestResult.Value!.Errors.Count);
    }
}

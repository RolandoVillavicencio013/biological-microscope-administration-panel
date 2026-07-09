using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Boxes;

public class UpdateBoxHandlerTests
{
    private const int ExistingId = 5;
    private const int ParentGroupId = 9;
    private const int ValidNumber = 8;
    private const string ValidName = "Updated name";

    private static Box ExistingBox() =>
        new(ExistingId, BoxNumber.Create(7), BoxName.Create("Cyanobacteria"), ParentGroupId);

    [Fact]
    public async Task HandleAsync_ExistingIdAndValidRequest_ReturnsOkWithUpdatedResponse()
    {
        // Arrange
        var request = new UpdateBoxRequest(ValidNumber, ValidName);
        var updated = new Box(ExistingId, BoxNumber.Create(ValidNumber), BoxName.Create(ValidName), ParentGroupId);
        var service = new Mock<IBoxService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingBox());
        service.Setup(s => s.UpdateAsync(It.IsAny<Box>())).ReturnsAsync(updated);

        // Act
        var result = await UpdateBoxHandler.HandleAsync(ExistingId, request, service.Object);

        // Assert
        var okResult = Assert.IsType<Ok<BoxResponse>>(result.Result);
        Assert.Equal(ValidName, okResult.Value!.Name);
    }

    [Fact]
    public async Task HandleAsync_NonExistingIdWithInvalidRequest_ReturnsNotFoundNotBadRequest()
    {
        // Arrange
        var request = new UpdateBoxRequest(0, "");
        var service = new Mock<IBoxService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync((Box?)null);

        // Act
        var result = await UpdateBoxHandler.HandleAsync(ExistingId, request, service.Object);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }

    [Fact]
    public async Task HandleAsync_ExistingIdWithInvalidRequest_ReturnsBadRequestWithValidationErrors()
    {
        // Arrange
        var request = new UpdateBoxRequest(0, "");
        var service = new Mock<IBoxService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingBox());

        // Act
        var result = await UpdateBoxHandler.HandleAsync(ExistingId, request, service.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        Assert.Equal(2, badRequestResult.Value!.Errors.Count);
    }

    [Fact]
    public async Task HandleAsync_ValidRequest_PreservesExistingGroupId()
    {
        // Arrange
        var request = new UpdateBoxRequest(ValidNumber, ValidName);
        var service = new Mock<IBoxService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingBox());
        Box? capturedBox = null;
        service.Setup(s => s.UpdateAsync(It.IsAny<Box>()))
            .Callback<Box>(box => capturedBox = box)
            .ReturnsAsync((Box box) => box);

        // Act
        await UpdateBoxHandler.HandleAsync(ExistingId, request, service.Object);

        // Assert
        Assert.Equal(ParentGroupId, capturedBox!.GroupId);
    }
}

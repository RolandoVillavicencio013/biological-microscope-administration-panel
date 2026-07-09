using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Boxes;

public class CreateBoxHandlerTests
{
    private const int ValidNumber = 7;
    private const string ValidName = "Cyanobacteria";
    private const int ParentGroupId = 9;
    private const int NewId = 5;

    [Fact]
    public async Task HandleAsync_ValidRequest_ReturnsCreatedWithMappedResponse()
    {
        // Arrange
        var request = new CreateBoxRequest(ValidNumber, ValidName, ParentGroupId);
        var created = new Box(NewId, BoxNumber.Create(ValidNumber), BoxName.Create(ValidName), ParentGroupId);
        var service = new Mock<IBoxService>();
        service.Setup(s => s.CreateAsync(It.IsAny<Box>())).ReturnsAsync(created);

        // Act
        var result = await CreateBoxHandler.HandleAsync(request, service.Object);

        // Assert
        var createdResult = Assert.IsType<Created<BoxResponse>>(result.Result);
        Assert.Equal(NewId, createdResult.Value!.Id);
    }

    [Fact]
    public async Task HandleAsync_InvalidRequest_ReturnsBadRequestWithValidationErrors()
    {
        // Arrange
        var request = new CreateBoxRequest(0, "", ParentGroupId);
        var service = new Mock<IBoxService>();

        // Act
        var result = await CreateBoxHandler.HandleAsync(request, service.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        Assert.Equal(2, badRequestResult.Value!.Errors.Count);
    }
}

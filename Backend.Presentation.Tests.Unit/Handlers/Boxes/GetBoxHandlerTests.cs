using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Boxes;

public class GetBoxHandlerTests
{
    private const int ExistingId = 5;
    private const int ParentGroupId = 9;

    [Fact]
    public async Task HandleAsync_ExistingId_ReturnsOkWithMappedResponse()
    {
        // Arrange
        var box = new Box(ExistingId, BoxNumber.Create(7), BoxName.Create("Cyanobacteria"), ParentGroupId);
        var service = new Mock<IBoxService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(box);

        // Act
        var result = await GetBoxHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        var okResult = Assert.IsType<Ok<BoxResponse>>(result.Result);
        Assert.Equal(ExistingId, okResult.Value!.Id);
    }

    [Fact]
    public async Task HandleAsync_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var service = new Mock<IBoxService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync((Box?)null);

        // Act
        var result = await GetBoxHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }
}

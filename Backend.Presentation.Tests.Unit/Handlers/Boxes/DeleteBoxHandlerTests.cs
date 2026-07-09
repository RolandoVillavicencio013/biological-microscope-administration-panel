using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Boxes;

public class DeleteBoxHandlerTests
{
    private const int ExistingId = 5;
    private const int ParentGroupId = 9;

    private static Box ExistingBox() =>
        new(ExistingId, BoxNumber.Create(7), BoxName.Create("Cyanobacteria"), ParentGroupId);

    [Fact]
    public async Task HandleAsync_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var service = new Mock<IBoxService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingBox());

        // Act
        var result = await DeleteBoxHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Fact]
    public async Task HandleAsync_ExistingId_CallsServiceDeleteAsyncOnce()
    {
        // Arrange
        var service = new Mock<IBoxService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingBox());

        // Act
        await DeleteBoxHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        service.Verify(s => s.DeleteAsync(ExistingId), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var service = new Mock<IBoxService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync((Box?)null);

        // Act
        var result = await DeleteBoxHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }

    [Fact]
    public async Task HandleAsync_NonExistingId_DoesNotCallServiceDeleteAsync()
    {
        // Arrange
        var service = new Mock<IBoxService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync((Box?)null);

        // Act
        await DeleteBoxHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        service.Verify(s => s.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Groups;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Groups;

public class DeleteGroupHandlerTests
{
    private const int ExistingId = 1;

    private static Group ExistingGroup() => new(ExistingId, GroupName.Create("Bacteria"));

    [Fact]
    public async Task HandleAsync_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var service = new Mock<IGroupService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingGroup());

        // Act
        var result = await DeleteGroupHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Fact]
    public async Task HandleAsync_ExistingId_CallsServiceDeleteAsyncOnce()
    {
        // Arrange
        var service = new Mock<IGroupService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingGroup());

        // Act
        await DeleteGroupHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        service.Verify(s => s.DeleteAsync(ExistingId), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var service = new Mock<IGroupService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync((Group?)null);

        // Act
        var result = await DeleteGroupHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }
}

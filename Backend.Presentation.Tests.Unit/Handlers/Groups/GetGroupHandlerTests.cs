using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Groups;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Groups;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Groups;

public class GetGroupHandlerTests
{
    private const int ExistingId = 1;

    [Fact]
    public async Task HandleAsync_ExistingId_ReturnsOkWithMappedResponse()
    {
        // Arrange
        var group = new Group(ExistingId, GroupName.Create("Bacteria"));
        var service = new Mock<IGroupService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(group);

        // Act
        var result = await GetGroupHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GroupResponse>>(result.Result);
        Assert.Equal(ExistingId, okResult.Value!.Id);
    }

    [Fact]
    public async Task HandleAsync_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var service = new Mock<IGroupService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync((Group?)null);

        // Act
        var result = await GetGroupHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }
}

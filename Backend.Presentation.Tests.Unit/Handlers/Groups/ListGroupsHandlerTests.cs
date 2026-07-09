using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Groups;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Groups;

public class ListGroupsHandlerTests
{
    private const int ExistingId = 1;

    [Fact]
    public async Task HandleAsync_ReturnsOkWithAllMappedGroups()
    {
        // Arrange
        var groups = new List<Group> { new(ExistingId, GroupName.Create("Bacteria")) };
        var service = new Mock<IGroupService>();
        service.Setup(s => s.ListAllAsync()).ReturnsAsync(groups);

        // Act
        var result = await ListGroupsHandler.HandleAsync(service.Object);

        // Assert
        Assert.Single(result.Value!);
    }
}

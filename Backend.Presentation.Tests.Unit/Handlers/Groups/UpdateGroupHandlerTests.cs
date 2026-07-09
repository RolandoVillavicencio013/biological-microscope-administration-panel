using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Groups;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Groups;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Groups;

public class UpdateGroupHandlerTests
{
    private const int ExistingId = 1;
    private const string ValidName = "Updated name";

    private static Group ExistingGroup() => new(ExistingId, GroupName.Create("Bacteria"));

    [Fact]
    public async Task HandleAsync_ExistingIdAndValidRequest_ReturnsOkWithUpdatedResponse()
    {
        // Arrange
        var request = new CreateGroupRequest(ValidName);
        var updated = new Group(ExistingId, GroupName.Create(ValidName));
        var service = new Mock<IGroupService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingGroup());
        service.Setup(s => s.UpdateAsync(It.IsAny<Group>())).ReturnsAsync(updated);

        // Act
        var result = await UpdateGroupHandler.HandleAsync(ExistingId, request, service.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GroupResponse>>(result.Result);
        Assert.Equal(ValidName, okResult.Value!.Name);
    }

    [Fact]
    public async Task HandleAsync_NonExistingIdWithInvalidRequest_ReturnsNotFoundNotBadRequest()
    {
        // Arrange
        var request = new CreateGroupRequest("");
        var service = new Mock<IGroupService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync((Group?)null);

        // Act
        var result = await UpdateGroupHandler.HandleAsync(ExistingId, request, service.Object);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }

    [Fact]
    public async Task HandleAsync_ExistingIdWithInvalidRequest_ReturnsBadRequestWithValidationErrors()
    {
        // Arrange
        var request = new CreateGroupRequest("");
        var service = new Mock<IGroupService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingGroup());

        // Act
        var result = await UpdateGroupHandler.HandleAsync(ExistingId, request, service.Object);

        // Assert
        Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Groups;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Groups;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Groups;

public class CreateGroupHandlerTests
{
    private const string ValidName = "Bacteria";
    private const int NewId = 1;

    [Fact]
    public async Task HandleAsync_ValidRequest_ReturnsCreatedWithMappedResponse()
    {
        // Arrange
        var request = new CreateGroupRequest(ValidName);
        var created = new Group(NewId, GroupName.Create(ValidName));
        var service = new Mock<IGroupService>();
        service.Setup(s => s.CreateAsync(It.IsAny<Group>())).ReturnsAsync(created);

        // Act
        var result = await CreateGroupHandler.HandleAsync(request, service.Object);

        // Assert
        var createdResult = Assert.IsType<Created<GroupResponse>>(result.Result);
        Assert.Equal(NewId, createdResult.Value!.Id);
    }

    [Fact]
    public async Task HandleAsync_InvalidRequest_ReturnsBadRequestWithValidationErrors()
    {
        // Arrange
        var request = new CreateGroupRequest("");
        var service = new Mock<IGroupService>();

        // Act
        var result = await CreateGroupHandler.HandleAsync(request, service.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        Assert.Single(badRequestResult.Value!.Errors);
    }
}

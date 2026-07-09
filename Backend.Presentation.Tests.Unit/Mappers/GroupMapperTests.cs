using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Groups;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Mappers;

public class GroupMapperTests
{
    private const int ExistingId = 1;
    private const string ValidName = "Bacteria";

    [Fact]
    public void ToResponse_ValidGroup_MapsAllFields()
    {
        // Arrange
        var group = new Group(ExistingId, GroupName.Create(ValidName));

        // Act
        var response = GroupMapper.ToResponse(group);

        // Assert
        Assert.Equal(new GroupResponse(ExistingId, ValidName), response);
    }

    [Fact]
    public void ToEntity_ValidRequest_ReturnsEntityWithMappedName()
    {
        // Arrange
        var request = new CreateGroupRequest(ValidName);

        // Act
        var group = GroupMapper.ToEntity(request);

        // Assert
        Assert.Equal(ValidName, group.Name.Value);
    }

    [Fact]
    public void ToEntity_InvalidName_ThrowsValidationExceptionWithNameFieldError()
    {
        // Arrange
        var request = new CreateGroupRequest("");

        // Act
        var exception = Assert.Throws<ValidationException>(() => GroupMapper.ToEntity(request));

        // Assert
        var error = Assert.Single(exception.Errors);
        Assert.Equal("Name", error.Field);
    }

    [Fact]
    public void ToUpdatedEntity_ValidRequest_PreservesId()
    {
        // Arrange
        var request = new CreateGroupRequest(ValidName);

        // Act
        var group = GroupMapper.ToUpdatedEntity(ExistingId, request);

        // Assert
        Assert.Equal(ExistingId, group.Id);
    }

    [Fact]
    public void ToUpdatedEntity_InvalidName_ThrowsValidationException()
    {
        // Arrange
        var request = new CreateGroupRequest("");

        // Act & Assert
        Assert.Throws<ValidationException>(() => GroupMapper.ToUpdatedEntity(ExistingId, request));
    }
}

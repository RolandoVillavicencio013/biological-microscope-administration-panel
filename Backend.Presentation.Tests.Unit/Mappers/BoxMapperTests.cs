using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Mappers;

public class BoxMapperTests
{
    private const int ExistingId = 5;
    private const int ValidNumber = 7;
    private const string ValidName = "Cyanobacteria";
    private const int ParentGroupId = 9;

    [Fact]
    public void ToResponse_ValidBox_MapsAllFields()
    {
        // Arrange
        var box = new Box(ExistingId, BoxNumber.Create(ValidNumber), BoxName.Create(ValidName), ParentGroupId);

        // Act
        var response = BoxMapper.ToResponse(box);

        // Assert
        Assert.Equal(new BoxResponse(ExistingId, ValidNumber, ValidName, ParentGroupId), response);
    }

    [Fact]
    public void ToEntity_ValidRequest_ReturnsEntityWithMappedValues()
    {
        // Arrange
        var request = new CreateBoxRequest(ValidNumber, ValidName, ParentGroupId);

        // Act
        var box = BoxMapper.ToEntity(request);

        // Assert
        Assert.Equal(ValidNumber, box.Number.Value);
        Assert.Equal(ValidName, box.Name.Value);
        Assert.Equal(ParentGroupId, box.GroupId);
    }

    [Fact]
    public void ToEntity_InvalidNumberAndName_ThrowsValidationExceptionWithBothErrors()
    {
        // Arrange
        var request = new CreateBoxRequest(0, "", ParentGroupId);

        // Act
        var exception = Assert.Throws<ValidationException>(() => BoxMapper.ToEntity(request));

        // Assert
        Assert.Equal(2, exception.Errors.Count);
    }

    [Fact]
    public void ToEntity_InvalidNumberOnly_ThrowsValidationExceptionWithNumberFieldError()
    {
        // Arrange
        var request = new CreateBoxRequest(0, ValidName, ParentGroupId);

        // Act
        var exception = Assert.Throws<ValidationException>(() => BoxMapper.ToEntity(request));

        // Assert
        var error = Assert.Single(exception.Errors);
        Assert.Equal("Number", error.Field);
    }

    [Fact]
    public void ToUpdatedEntity_ValidRequest_PreservesIdAndGroupId()
    {
        // Arrange
        var request = new UpdateBoxRequest(ValidNumber, ValidName);

        // Act
        var box = BoxMapper.ToUpdatedEntity(ExistingId, ParentGroupId, request);

        // Assert
        Assert.Equal(ExistingId, box.Id);
        Assert.Equal(ParentGroupId, box.GroupId);
    }

    [Fact]
    public void ToUpdatedEntity_InvalidName_ThrowsValidationException()
    {
        // Arrange
        var request = new UpdateBoxRequest(ValidNumber, "");

        // Act & Assert
        Assert.Throws<ValidationException>(() => BoxMapper.ToUpdatedEntity(ExistingId, ParentGroupId, request));
    }
}

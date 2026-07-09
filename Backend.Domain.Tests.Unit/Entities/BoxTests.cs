using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.Entities;

public class BoxTests
{
    private const int ExistingId = 5;
    private const int ParentGroupId = 9;

    private static BoxNumber ValidNumber() => BoxNumber.Create(7);
    private static BoxName ValidName() => BoxName.Create("Cyanobacteria");

    [Fact]
    public void Constructor_WithId_AssignsAllProperties()
    {
        // Arrange
        var number = ValidNumber();
        var name = ValidName();

        // Act
        var box = new Box(ExistingId, number, name, ParentGroupId);

        // Assert
        Assert.Equal(ExistingId, box.Id);
        Assert.Equal(number, box.Number);
        Assert.Equal(name, box.Name);
        Assert.Equal(ParentGroupId, box.GroupId);
    }

    [Fact]
    public void Constructor_WithoutId_DefaultsIdToZero()
    {
        // Act
        var box = new Box(ValidNumber(), ValidName(), ParentGroupId);

        // Assert
        Assert.Equal(0, box.Id);
    }

    [Fact]
    public void Constructor_WithoutId_AssignsGroupId()
    {
        // Act
        var box = new Box(ValidNumber(), ValidName(), ParentGroupId);

        // Assert
        Assert.Equal(ParentGroupId, box.GroupId);
    }

    [Fact]
    public void Constructor_NewInstance_SlidesCollectionIsEmpty()
    {
        // Act
        var box = new Box(ValidNumber(), ValidName(), ParentGroupId);

        // Assert
        Assert.Empty(box.Slides);
    }
}

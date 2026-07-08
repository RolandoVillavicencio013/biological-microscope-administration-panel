using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.Entities;

public class GroupTests
{
    private const int ExistingId = 42;

    private static GroupName ValidName() => GroupName.Create("Bacteria");

    [Fact]
    public void Constructor_WithId_AssignsIdAndName()
    {
        // Arrange
        var name = ValidName();

        // Act
        var group = new Group(ExistingId, name);

        // Assert
        Assert.Equal(ExistingId, group.Id);
        Assert.Equal(name, group.Name);
    }

    [Fact]
    public void Constructor_WithoutId_DefaultsIdToZero()
    {
        // Arrange
        var name = ValidName();

        // Act
        var group = new Group(name);

        // Assert
        Assert.Equal(0, group.Id);
    }

    [Fact]
    public void Constructor_WithoutId_AssignsName()
    {
        // Arrange
        var name = ValidName();

        // Act
        var group = new Group(name);

        // Assert
        Assert.Equal(name, group.Name);
    }

    [Fact]
    public void Constructor_NewInstance_BoxesCollectionIsEmpty()
    {
        // Act
        var group = new Group(ValidName());

        // Assert
        Assert.Empty(group.Boxes);
    }
}

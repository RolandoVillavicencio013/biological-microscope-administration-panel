using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.Entities;

public class SlideTests
{
    private const int ExistingId = 3;
    private const int ParentBoxId = 11;

    private static SlideCode ValidCode() => SlideCode.Create("1TB07");
    private static SlideName ValidName() => SlideName.Create("Gloecapsa");
    private static Text ValidText(string value) => Text.Create(value);

    [Fact]
    public void Constructor_WithId_AssignsAllProperties()
    {
        // Arrange
        var code = ValidCode();
        var name = ValidName();
        var description = ValidText("Description");
        var observations = ValidText("Observations");

        // Act
        var slide = new Slide(ExistingId, code, name, ParentBoxId, description, observations);

        // Assert
        Assert.Equal(ExistingId, slide.Id);
        Assert.Equal(code, slide.Code);
        Assert.Equal(name, slide.Name);
        Assert.Equal(ParentBoxId, slide.BoxId);
        Assert.Equal(description, slide.Description);
        Assert.Equal(observations, slide.Observations);
    }

    [Fact]
    public void Constructor_WithoutId_DefaultsIdToZero()
    {
        // Act
        var slide = new Slide(ValidCode(), ValidName(), ParentBoxId);

        // Assert
        Assert.Equal(0, slide.Id);
    }

    [Fact]
    public void Constructor_WithoutOptionalFields_DescriptionDefaultsToNull()
    {
        // Act
        var slide = new Slide(ValidCode(), ValidName(), ParentBoxId);

        // Assert
        Assert.Null(slide.Description);
    }

    [Fact]
    public void Constructor_WithoutOptionalFields_ObservationsDefaultsToNull()
    {
        // Act
        var slide = new Slide(ValidCode(), ValidName(), ParentBoxId);

        // Assert
        Assert.Null(slide.Observations);
    }

    [Fact]
    public void Constructor_NewInstance_ImagesCollectionIsEmpty()
    {
        // Act
        var slide = new Slide(ValidCode(), ValidName(), ParentBoxId);

        // Assert
        Assert.Empty(slide.Images);
    }

    [Fact]
    public void Description_SetAfterConstruction_UpdatesValue()
    {
        // Arrange
        var slide = new Slide(ValidCode(), ValidName(), ParentBoxId);
        var newDescription = ValidText("Updated description");

        // Act
        slide.Description = newDescription;

        // Assert
        Assert.Equal(newDescription, slide.Description);
    }

    [Fact]
    public void Observations_SetToNullAfterConstruction_ClearsValue()
    {
        // Arrange
        var slide = new Slide(ExistingId, ValidCode(), ValidName(), ParentBoxId, observations: ValidText("Initial"));

        // Act
        slide.Observations = null;

        // Assert
        Assert.Null(slide.Observations);
    }
}

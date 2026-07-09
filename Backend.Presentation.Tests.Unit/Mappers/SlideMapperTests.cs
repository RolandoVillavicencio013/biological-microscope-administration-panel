using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Mappers;

public class SlideMapperTests
{
    private const int ExistingId = 3;
    private const string ValidCode = "1TB07";
    private const string ValidName = "Gloecapsa";
    private const int ParentBoxId = 11;
    private const string ValidDescription = "A description";
    private const string ValidObservations = "Some observations";

    private static Slide NewSlide(Text? description = null, Text? observations = null) =>
        new(ExistingId, SlideCode.Create(ValidCode), SlideName.Create(ValidName), ParentBoxId, description, observations);

    [Fact]
    public void ToResponse_ValidSlide_MapsFieldsWithoutImages()
    {
        // Arrange
        var slide = NewSlide(Text.Create(ValidDescription));

        // Act
        var response = SlideMapper.ToResponse(slide);

        // Assert
        Assert.Equal(new SlideResponse(ExistingId, ValidCode, ValidName, ParentBoxId, ValidDescription, null), response);
    }

    [Fact]
    public void ToDetailResponse_SlideWithImages_MapsImagesToResponses()
    {
        // Arrange
        var slide = NewSlide();
        var image = new Image(ImageUrl.Create("/img.jpg"), ImageFileName.Create("img.jpg"), ImageOrder.Create(1), ExistingId);
        slide.Images.Add(image);

        // Act
        var response = SlideMapper.ToDetailResponse(slide);

        // Assert
        var mappedImage = Assert.Single(response.Images);
        Assert.Equal(image.FileName.Value, mappedImage.FileName);
    }

    [Fact]
    public void ToEntity_RequestWithoutOptionalFields_DescriptionIsNull()
    {
        // Arrange
        var request = new CreateSlideRequest(ValidCode, ValidName, ParentBoxId, null, null);

        // Act
        var slide = SlideMapper.ToEntity(request);

        // Assert
        Assert.Null(slide.Description);
    }

    [Fact]
    public void ToEntity_RequestWithOptionalFields_MapsDescriptionAndObservations()
    {
        // Arrange
        var request = new CreateSlideRequest(ValidCode, ValidName, ParentBoxId, ValidDescription, ValidObservations);

        // Act
        var slide = SlideMapper.ToEntity(request);

        // Assert
        Assert.Equal(ValidDescription, slide.Description?.Value);
        Assert.Equal(ValidObservations, slide.Observations?.Value);
    }

    [Fact]
    public void ToEntity_WhitespaceOnlyDescription_ThrowsValidationException()
    {
        // Arrange
        var request = new CreateSlideRequest(ValidCode, ValidName, ParentBoxId, "   ", null);

        // Act & Assert
        Assert.Throws<ValidationException>(() => SlideMapper.ToEntity(request));
    }

    [Fact]
    public void ToEntity_InvalidCodeAndName_ThrowsValidationExceptionWithBothErrors()
    {
        // Arrange
        var request = new CreateSlideRequest("", "", ParentBoxId, null, null);

        // Act
        var exception = Assert.Throws<ValidationException>(() => SlideMapper.ToEntity(request));

        // Assert
        Assert.Equal(2, exception.Errors.Count);
    }

    [Fact]
    public void ApplyUpdate_ValidRequest_UpdatesDescriptionAndObservations()
    {
        // Arrange
        var slide = NewSlide();
        var request = new UpdateSlideRequest(ValidDescription, ValidObservations);

        // Act
        SlideMapper.ApplyUpdate(slide, request);

        // Assert
        Assert.Equal(ValidDescription, slide.Description?.Value);
        Assert.Equal(ValidObservations, slide.Observations?.Value);
    }

    [Fact]
    public void ApplyUpdate_NullDescriptionInRequest_ClearsExistingDescription()
    {
        // Arrange
        var slide = NewSlide(description: Text.Create(ValidDescription));
        var request = new UpdateSlideRequest(null, null);

        // Act
        SlideMapper.ApplyUpdate(slide, request);

        // Assert
        Assert.Null(slide.Description);
    }

    [Fact]
    public void ApplyUpdate_WhitespaceOnlyObservations_ThrowsValidationException()
    {
        // Arrange
        var slide = NewSlide();
        var request = new UpdateSlideRequest(null, "   ");

        // Act & Assert
        Assert.Throws<ValidationException>(() => SlideMapper.ApplyUpdate(slide, request));
    }
}

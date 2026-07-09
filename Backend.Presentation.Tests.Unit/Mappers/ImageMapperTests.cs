using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Mappers;

public class ImageMapperTests
{
    private const int ExistingId = 21;
    private const string ValidUrl = "/uploads/slides/4/sample.jpg";
    private const string ValidFileName = "sample.jpg";
    private const int ParentSlideId = 4;

    private static Image NewImage(int order = 1) => new(
        ExistingId, ImageUrl.Create(ValidUrl), ImageFileName.Create(ValidFileName),
        ImageOrder.Create(order), DateTime.UtcNow, ParentSlideId);

    [Fact]
    public void ToResponse_ValidImage_MapsAllFields()
    {
        // Arrange
        var image = NewImage();

        // Act
        var response = ImageMapper.ToResponse(image);

        // Assert
        Assert.Equal(image.Id, response.Id);
        Assert.Equal(image.FileUrl.Value, response.FileUrl);
        Assert.Equal(image.FileName.Value, response.FileName);
        Assert.Equal(image.Order.Value, response.Order);
        Assert.Equal(image.SlideId, response.SlideId);
    }

    [Fact]
    public void ApplyOrderUpdate_ValidOrder_UpdatesImageOrder()
    {
        // Arrange
        var image = NewImage(order: 1);
        const int newOrder = 6;

        // Act
        ImageMapper.ApplyOrderUpdate(image, newOrder);

        // Assert
        Assert.Equal(newOrder, image.Order.Value);
    }

    [Fact]
    public void ApplyOrderUpdate_OrderOutsideRange_ThrowsValidationExceptionWithSingleEmptyFieldError()
    {
        // Arrange
        var image = NewImage();

        // Act
        var exception = Assert.Throws<ValidationException>(() => ImageMapper.ApplyOrderUpdate(image, ImageOrder.Max + 1));

        // Assert
        var error = Assert.Single(exception.Errors);
        Assert.Equal(string.Empty, error.Field);
    }
}

using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.Entities;

public class ImageTests
{
    private const int ExistingId = 21;
    private const int ParentSlideId = 4;
    private static readonly TimeSpan ClockTolerance = TimeSpan.FromSeconds(5);

    private static ImageUrl ValidUrl() => ImageUrl.Create("/uploads/slides/4/sample.jpg");
    private static ImageFileName ValidFileName() => ImageFileName.Create("sample.jpg");
    private static ImageOrder ValidOrder() => ImageOrder.Create(1);

    [Fact]
    public void Constructor_WithId_AssignsGivenUploadedAt()
    {
        // Arrange
        var uploadedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Act
        var image = new Image(ExistingId, ValidUrl(), ValidFileName(), ValidOrder(), uploadedAt, ParentSlideId);

        // Assert
        Assert.Equal(uploadedAt, image.UploadedAt);
    }

    [Fact]
    public void Constructor_WithId_AssignsAllProperties()
    {
        // Arrange
        var url = ValidUrl();
        var fileName = ValidFileName();
        var order = ValidOrder();
        var uploadedAt = DateTime.UtcNow;

        // Act
        var image = new Image(ExistingId, url, fileName, order, uploadedAt, ParentSlideId);

        // Assert
        Assert.Equal(ExistingId, image.Id);
        Assert.Equal(url, image.FileUrl);
        Assert.Equal(fileName, image.FileName);
        Assert.Equal(order, image.Order);
        Assert.Equal(ParentSlideId, image.SlideId);
    }

    [Fact]
    public void Constructor_WithoutId_DefaultsIdToZero()
    {
        // Act
        var image = new Image(ValidUrl(), ValidFileName(), ValidOrder(), ParentSlideId);

        // Assert
        Assert.Equal(0, image.Id);
    }

    [Fact]
    public void Constructor_WithoutId_StampsUploadedAtCloseToNow()
    {
        // Act
        var image = new Image(ValidUrl(), ValidFileName(), ValidOrder(), ParentSlideId);

        // Assert
        Assert.True(DateTime.UtcNow - image.UploadedAt < ClockTolerance);
    }

    [Fact]
    public void Order_SetAfterConstruction_UpdatesValue()
    {
        // Arrange
        var image = new Image(ValidUrl(), ValidFileName(), ValidOrder(), ParentSlideId);
        var newOrder = ImageOrder.Create(6);

        // Act
        image.Order = newOrder;

        // Assert
        Assert.Equal(newOrder, image.Order);
    }
}

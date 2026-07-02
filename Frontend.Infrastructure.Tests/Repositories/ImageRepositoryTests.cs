using Moq;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Images;
using UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.ApiClient;
using UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.Repositories;
using Xunit;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.Tests.Repositories;

/// <summary>Unit tests for <see cref="ImageRepository"/>.</summary>
public class ImageRepositoryTests
{
    private readonly Mock<BioMicroscopeApiClient> _mockApiClient;
    private readonly ImageRepository _repository;

    public ImageRepositoryTests()
    {
        _mockApiClient = new Mock<BioMicroscopeApiClient>();

        var mockHttpClient = new Mock<HttpClient>();
        mockHttpClient.Setup(x => x.BaseAddress).Returns(new Uri("https://localhost:7121"));

        _mockApiClient.Setup(x => x.Http).Returns(mockHttpClient.Object);
        _repository = new ImageRepository(_mockApiClient.Object);
    }

    [Fact]
    public async Task ListBySlideAsync_WithValidSlideId_ReturnsImageList()
    {
        // Arrange
        int slideId = 1;
        var imageResponses = new List<ImageResponse>
        {
            new(1, "/uploads/image1.jpg", "image1.jpg", 1, DateTime.UtcNow, slideId),
            new(2, "/uploads/image2.jpg", "image2.jpg", 2, DateTime.UtcNow, slideId)
        };

        _mockApiClient.Setup(c => c.GetImagesAsync(slideId))
            .ReturnsAsync(imageResponses);

        // Act
        var result = await _repository.ListBySlideAsync(slideId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, img =>
        {
            Assert.StartsWith("https://localhost:7121", img.FileUrl);
        });
        _mockApiClient.Verify(c => c.GetImagesAsync(slideId), Times.Once);
    }

    [Fact]
    public async Task ListBySlideAsync_WithNoImages_ReturnsEmptyList()
    {
        // Arrange
        int slideId = 999;
        _mockApiClient.Setup(c => c.GetImagesAsync(slideId))
            .ReturnsAsync((List<ImageResponse>?)null);

        // Act
        var result = await _repository.ListBySlideAsync(slideId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task ListBySlideAsync_ConstructsFullUrlWithBaseAddress()
    {
        // Arrange
        int slideId = 1;
        var relativeFileUrl = "/uploads/test-image.jpg";
        var imageResponse = new ImageResponse(1, relativeFileUrl, "test-image.jpg", 1, DateTime.UtcNow, slideId);

        _mockApiClient.Setup(c => c.GetImagesAsync(slideId))
            .ReturnsAsync(new List<ImageResponse> { imageResponse });

        // Act
        var result = await _repository.ListBySlideAsync(slideId);

        // Assert
        Assert.Single(result);
        Assert.Equal("https://localhost:7121/uploads/test-image.jpg", result[0].FileUrl);
    }

    [Fact]
    public async Task ListBySlideAsync_WithAbsoluteUrl_DoesNotModify()
    {
        // Arrange
        int slideId = 1;
        var absoluteUrl = "https://example.com/uploads/image.jpg";
        var imageResponse = new ImageResponse(1, absoluteUrl, "image.jpg", 1, DateTime.UtcNow, slideId);

        _mockApiClient.Setup(c => c.GetImagesAsync(slideId))
            .ReturnsAsync(new List<ImageResponse> { imageResponse });

        // Act
        var result = await _repository.ListBySlideAsync(slideId);

        // Assert
        Assert.Single(result);
        Assert.Equal(absoluteUrl, result[0].FileUrl);
    }

    [Fact]
    public async Task UploadAsync_WithValidParameters_ReturnsUploadedImage()
    {
        // Arrange
        int slideId = 1;
        var fileContent = "test image content";
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
        var fileName = "test.jpg";
        int order = 1;

        var imageResponse = new ImageResponse(1, "/uploads/guid_test.jpg", fileName, order, DateTime.UtcNow, slideId);

        _mockApiClient.Setup(c => c.UploadImageAsync(slideId, stream, fileName, order))
            .ReturnsAsync(imageResponse);

        // Act
        var result = await _repository.UploadAsync(slideId, stream, fileName, order);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(fileName, result.FileName);
        Assert.StartsWith("https://localhost:7121", result.FileUrl);
        _mockApiClient.Verify(c => c.UploadImageAsync(slideId, stream, fileName, order), Times.Once);
    }

    [Fact]
    public async Task UpdateOrderAsync_WithValidParameters_ReturnsUpdatedImage()
    {
        // Arrange
        int imageId = 1;
        var updateDto = new UpdateImageOrderDto { Order = 3 };
        var imageResponse = new ImageResponse(imageId, "/uploads/image.jpg", "image.jpg", 3, DateTime.UtcNow, 1);

        _mockApiClient.Setup(c => c.UpdateImageOrderAsync(imageId, It.IsAny<UpdateImageOrderRequest>()))
            .ReturnsAsync(imageResponse);

        // Act
        var result = await _repository.UpdateOrderAsync(imageId, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Order);
        _mockApiClient.Verify(c => c.UpdateImageOrderAsync(imageId, It.IsAny<UpdateImageOrderRequest>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithValidImageId_CallsApiClient()
    {
        // Arrange
        int imageId = 1;
        _mockApiClient.Setup(c => c.DeleteImageAsync(imageId))
            .Returns(Task.CompletedTask);

        // Act
        await _repository.DeleteAsync(imageId);

        // Assert
        _mockApiClient.Verify(c => c.DeleteImageAsync(imageId), Times.Once);
    }
}

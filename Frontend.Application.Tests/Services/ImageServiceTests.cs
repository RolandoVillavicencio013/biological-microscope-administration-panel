using Moq;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Images;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Services.Implementations;
using Xunit;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Tests.Services;

/// <summary>Unit tests for <see cref="ImageService"/>.</summary>
public class ImageServiceTests
{
    private readonly Mock<IImageRepository> _mockRepository;
    private readonly ImageService _service;

    public ImageServiceTests()
    {
        _mockRepository = new Mock<IImageRepository>();
        _service = new ImageService(_mockRepository.Object);
    }

    [Fact]
    public async Task ListBySlideAsync_WithValidSlideId_ReturnsImageList()
    {
        // Arrange
        int slideId = 1;
        var imageDtos = new List<ImageDto>
        {
            new(1, "https://localhost:7121/uploads/image1.jpg", "image1.jpg", 1, DateTime.UtcNow, slideId),
            new(2, "https://localhost:7121/uploads/image2.jpg", "image2.jpg", 2, DateTime.UtcNow, slideId)
        };

        _mockRepository.Setup(r => r.ListBySlideAsync(slideId))
            .ReturnsAsync(imageDtos);

        // Act
        var result = await _service.ListBySlideAsync(slideId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(imageDtos, result);
        _mockRepository.Verify(r => r.ListBySlideAsync(slideId), Times.Once);
    }

    [Fact]
    public async Task ListBySlideAsync_WithNoImages_ReturnsEmptyList()
    {
        // Arrange
        int slideId = 999;
        _mockRepository.Setup(r => r.ListBySlideAsync(slideId))
            .ReturnsAsync(new List<ImageDto>());

        // Act
        var result = await _service.ListBySlideAsync(slideId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task UploadAsync_WithValidParameters_ReturnsImageDto()
    {
        // Arrange
        int slideId = 1;
        var fileContent = "test image";
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
        var fileName = "test.jpg";
        int order = 1;

        var expectedImageDto = new ImageDto(1, "https://localhost:7121/uploads/guid_test.jpg", fileName, order, DateTime.UtcNow, slideId);

        _mockRepository.Setup(r => r.UploadAsync(slideId, stream, fileName, order))
            .ReturnsAsync(expectedImageDto);

        // Act
        var result = await _service.UploadAsync(slideId, stream, fileName, order);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedImageDto, result);
        _mockRepository.Verify(r => r.UploadAsync(slideId, stream, fileName, order), Times.Once);
    }

    [Fact]
    public async Task UpdateOrderAsync_WithValidParameters_ReturnsUpdatedImage()
    {
        // Arrange
        int imageId = 1;
        var updateDto = new UpdateImageOrderDto { Order = 2 };
        var expectedImageDto = new ImageDto(imageId, "https://localhost:7121/uploads/image.jpg", "image.jpg", 2, DateTime.UtcNow, 1);

        _mockRepository.Setup(r => r.UpdateOrderAsync(imageId, updateDto))
            .ReturnsAsync(expectedImageDto);

        // Act
        var result = await _service.UpdateOrderAsync(imageId, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Order);
        _mockRepository.Verify(r => r.UpdateOrderAsync(imageId, updateDto), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithValidImageId_CallsRepository()
    {
        // Arrange
        int imageId = 1;
        _mockRepository.Setup(r => r.DeleteAsync(imageId))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(imageId);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(imageId), Times.Once);
    }
}

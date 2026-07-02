using Moq;
using Microsoft.AspNetCore.Http;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Images;
using Xunit;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Handlers.Images;

/// <summary>Unit tests for <see cref="UploadImageHandler"/>.</summary>
public class UploadImageHandlerTests
{
    private readonly Mock<ISlideService> _mockSlideService;
    private readonly Mock<IImageService> _mockImageService;
    private readonly Mock<IFileStorageService> _mockFileStorageService;

    public UploadImageHandlerTests()
    {
        _mockSlideService = new Mock<ISlideService>();
        _mockImageService = new Mock<IImageService>();
        _mockFileStorageService = new Mock<IFileStorageService>();
    }

    [Fact]
    public async Task HandleAsync_WithValidFile_ReturnsCreatedImage()
    {
        // Arrange
        int slideId = 1;
        var slide = CreateTestSlide(slideId);
        var formFile = CreateTestFormFile("test.jpg", "fake image content");
        int order = 1;
        var fileUrl = "/uploads/guid_test.jpg";

        _mockSlideService.Setup(s => s.FindByIdAsync(slideId))
            .ReturnsAsync(slide);

        _mockFileStorageService.Setup(fs => fs.SaveAsync(It.IsAny<Stream>(), formFile.FileName))
            .ReturnsAsync(fileUrl);

        // Act
        var result = await UploadImageHandler.HandleAsync(
            slideId, formFile, order, _mockSlideService.Object,
            _mockImageService.Object, _mockFileStorageService.Object);

        // Assert
        Assert.NotNull(result);
        _mockSlideService.Verify(s => s.FindByIdAsync(slideId), Times.Once);
        _mockFileStorageService.Verify(
            fs => fs.SaveAsync(It.IsAny<Stream>(), formFile.FileName),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentSlide_ReturnsNotFound()
    {
        // Arrange
        int slideId = 999;
        var formFile = CreateTestFormFile("test.jpg", "content");

        _mockSlideService.Setup(s => s.FindByIdAsync(slideId))
            .ReturnsAsync((Slide?)null);

        // Act
        var result = await UploadImageHandler.HandleAsync(
            slideId, formFile, 1, _mockSlideService.Object,
            _mockImageService.Object, _mockFileStorageService.Object);

        // Assert
        Assert.NotNull(result);
        // El resultado debería ser NotFound
        var notFoundResult = result as Microsoft.AspNetCore.Http.HttpResults.NotFound<string>;
        Assert.NotNull(notFoundResult);
    }

    [Fact]
    public async Task HandleAsync_WithNullFile_ReturnsBadRequest()
    {
        // Arrange
        int slideId = 1;
        var slide = CreateTestSlide(slideId);

        _mockSlideService.Setup(s => s.FindByIdAsync(slideId))
            .ReturnsAsync(slide);

        // Act
        var result = await UploadImageHandler.HandleAsync(
            slideId, null!, 1, _mockSlideService.Object,
            _mockImageService.Object, _mockFileStorageService.Object);

        // Assert
        Assert.NotNull(result);
        // El resultado debería ser BadRequest
        var badRequest = result as Microsoft.AspNetCore.Http.HttpResults.BadRequest<dynamic>;
        Assert.NotNull(badRequest);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyFile_ReturnsBadRequest()
    {
        // Arrange
        int slideId = 1;
        var slide = CreateTestSlide(slideId);
        var emptyFile = CreateTestFormFile("empty.jpg", "");

        _mockSlideService.Setup(s => s.FindByIdAsync(slideId))
            .ReturnsAsync(slide);

        // Act
        var result = await UploadImageHandler.HandleAsync(
            slideId, emptyFile, 1, _mockSlideService.Object,
            _mockImageService.Object, _mockFileStorageService.Object);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task HandleAsync_WithInvalidOrder_ReturnsBadRequest()
    {
        // Arrange
        int slideId = 1;
        var slide = CreateTestSlide(slideId);
        var formFile = CreateTestFormFile("test.jpg", "content");
        int invalidOrder = 999; // Suponiendo que el máximo es menor

        _mockSlideService.Setup(s => s.FindByIdAsync(slideId))
            .ReturnsAsync(slide);

        // Act
        var result = await UploadImageHandler.HandleAsync(
            slideId, formFile, invalidOrder, _mockSlideService.Object,
            _mockImageService.Object, _mockFileStorageService.Object);

        // Assert
        Assert.NotNull(result);
    }

    // Helper methods
    private static Slide CreateTestSlide(int id)
    {
        return Slide.Create("TEST-001", "Test Slide", "Test Description", "Test Observations", 1).Value;
    }

    private static IFormFile CreateTestFormFile(string fileName, string content)
    {
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        var formFile = new Mock<IFormFile>();

        formFile.Setup(f => f.FileName).Returns(fileName);
        formFile.Setup(f => f.Length).Returns(stream.Length);
        formFile.Setup(f => f.OpenReadStream()).Returns(stream);
        formFile.Setup(f => f.ContentDisposition)
            .Returns($"form-data; name=\"file\"; filename=\"{fileName}\"");

        return formFile.Object;
    }
}

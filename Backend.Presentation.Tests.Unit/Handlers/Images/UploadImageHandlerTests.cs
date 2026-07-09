using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Images;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Images;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Images;

public class UploadImageHandlerTests
{
    private const int SlideId = 4;
    private const int ValidOrder = 1;
    private const string ValidFileName = "sample.jpg";
    private const string SavedFileUrl = "/uploads/slides/4/sample.jpg";

    private static Slide ExistingSlide() =>
        new(SlideCode.Create("1TB07"), SlideName.Create("Gloecapsa"), SlideId);

    private static Mock<IFormFile> NonEmptyFile(string fileName = ValidFileName)
    {
        var file = new Mock<IFormFile>();
        file.SetupGet(f => f.Length).Returns(3);
        file.SetupGet(f => f.FileName).Returns(fileName);
        file.Setup(f => f.OpenReadStream()).Returns(new MemoryStream([1, 2, 3]));
        return file;
    }

    private static Mock<IFileStorageService> FileStorageReturning(string url)
    {
        var fileStorageService = new Mock<IFileStorageService>();
        fileStorageService.Setup(f => f.SaveAsync(It.IsAny<Stream>(), It.IsAny<string>())).ReturnsAsync(url);
        return fileStorageService;
    }

    [Fact]
    public async Task HandleAsync_SlideNotFound_ReturnsNotFound()
    {
        // Arrange
        var slideService = new Mock<ISlideService>();
        slideService.Setup(s => s.FindByIdAsync(SlideId)).ReturnsAsync((Slide?)null);
        var imageService = new Mock<IImageService>();
        var fileStorageService = new Mock<IFileStorageService>();

        // Act
        var result = await UploadImageHandler.HandleAsync(
            SlideId, NonEmptyFile().Object, ValidOrder,
            slideService.Object, imageService.Object, fileStorageService.Object);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }

    [Fact]
    public async Task HandleAsync_SlideNotFound_DoesNotCallFileStorageService()
    {
        // Arrange
        var slideService = new Mock<ISlideService>();
        slideService.Setup(s => s.FindByIdAsync(SlideId)).ReturnsAsync((Slide?)null);
        var imageService = new Mock<IImageService>();
        var fileStorageService = new Mock<IFileStorageService>();

        // Act
        await UploadImageHandler.HandleAsync(
            SlideId, NonEmptyFile().Object, ValidOrder,
            slideService.Object, imageService.Object, fileStorageService.Object);

        // Assert
        fileStorageService.Verify(f => f.SaveAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_EmptyFile_ReturnsBadRequestWithFileError()
    {
        // Arrange
        var slideService = new Mock<ISlideService>();
        slideService.Setup(s => s.FindByIdAsync(SlideId)).ReturnsAsync(ExistingSlide());
        var emptyFile = new Mock<IFormFile>();
        emptyFile.SetupGet(f => f.Length).Returns(0);
        var imageService = new Mock<IImageService>();
        var fileStorageService = new Mock<IFileStorageService>();

        // Act
        var result = await UploadImageHandler.HandleAsync(
            SlideId, emptyFile.Object, ValidOrder,
            slideService.Object, imageService.Object, fileStorageService.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        Assert.Contains(badRequestResult.Value!.Errors, e => e.Field == "File");
    }

    [Fact]
    public async Task HandleAsync_OrderOutsideRange_ReturnsBadRequestWithOrderError()
    {
        // Arrange
        var slideService = new Mock<ISlideService>();
        slideService.Setup(s => s.FindByIdAsync(SlideId)).ReturnsAsync(ExistingSlide());
        var imageService = new Mock<IImageService>();
        var fileStorageService = new Mock<IFileStorageService>();

        // Act
        var result = await UploadImageHandler.HandleAsync(
            SlideId, NonEmptyFile().Object, ImageOrder.Max + 1,
            slideService.Object, imageService.Object, fileStorageService.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        Assert.Contains(badRequestResult.Value!.Errors, e => e.Field == "Order");
    }

    [Fact]
    public async Task HandleAsync_EmptyFileAndInvalidOrder_ReturnsBadRequestWithBothErrors()
    {
        // Arrange
        var slideService = new Mock<ISlideService>();
        slideService.Setup(s => s.FindByIdAsync(SlideId)).ReturnsAsync(ExistingSlide());
        var emptyFile = new Mock<IFormFile>();
        emptyFile.SetupGet(f => f.Length).Returns(0);
        var imageService = new Mock<IImageService>();
        var fileStorageService = new Mock<IFileStorageService>();

        // Act
        var result = await UploadImageHandler.HandleAsync(
            SlideId, emptyFile.Object, ImageOrder.Max + 1,
            slideService.Object, imageService.Object, fileStorageService.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        Assert.Equal(2, badRequestResult.Value!.Errors.Count);
    }

    [Fact]
    public async Task HandleAsync_ValidFileAndOrder_ReturnsCreatedWithSlideImagesLocation()
    {
        // Arrange
        var slideService = new Mock<ISlideService>();
        slideService.Setup(s => s.FindByIdAsync(SlideId)).ReturnsAsync(ExistingSlide());
        var imageService = new Mock<IImageService>();
        imageService.Setup(s => s.CreateAsync(It.IsAny<Image>())).ReturnsAsync((Image image) => image);
        var fileStorageService = FileStorageReturning(SavedFileUrl);

        // Act
        var result = await UploadImageHandler.HandleAsync(
            SlideId, NonEmptyFile().Object, ValidOrder,
            slideService.Object, imageService.Object, fileStorageService.Object);

        // Assert
        var createdResult = Assert.IsType<Created<ImageResponse>>(result.Result);
        Assert.Equal($"/slides/{SlideId}/images", createdResult.Location);
    }

    [Fact]
    public async Task HandleAsync_FileStorageReturnsUrlExceedingMaxLength_ReturnsBadRequestWithFileError()
    {
        // Arrange
        var slideService = new Mock<ISlideService>();
        slideService.Setup(s => s.FindByIdAsync(SlideId)).ReturnsAsync(ExistingSlide());
        var imageService = new Mock<IImageService>();
        var overLengthUrl = new string('a', ImageUrl.MaxLength + 1);
        var fileStorageService = FileStorageReturning(overLengthUrl);

        // Act
        var result = await UploadImageHandler.HandleAsync(
            SlideId, NonEmptyFile().Object, ValidOrder,
            slideService.Object, imageService.Object, fileStorageService.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        Assert.Contains(badRequestResult.Value!.Errors, e => e.Field == "File");
    }
}

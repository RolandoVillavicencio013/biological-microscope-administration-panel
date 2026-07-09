using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Images;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Images;

public class DeleteImageHandlerTests
{
    private const int ExistingId = 21;
    private const int ParentSlideId = 4;

    private static Image ExistingImage() => new(
        ExistingId, ImageUrl.Create("/img.jpg"), ImageFileName.Create("img.jpg"),
        ImageOrder.Create(1), DateTime.UtcNow, ParentSlideId);

    [Fact]
    public async Task HandleAsync_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var service = new Mock<IImageService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingImage());

        // Act
        var result = await DeleteImageHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Fact]
    public async Task HandleAsync_ExistingId_CallsServiceDeleteAsyncOnce()
    {
        // Arrange
        var service = new Mock<IImageService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync(ExistingImage());

        // Act
        await DeleteImageHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        service.Verify(s => s.DeleteAsync(ExistingId), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var service = new Mock<IImageService>();
        service.Setup(s => s.FindByIdAsync(ExistingId)).ReturnsAsync((Image?)null);

        // Act
        var result = await DeleteImageHandler.HandleAsync(ExistingId, service.Object);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }
}

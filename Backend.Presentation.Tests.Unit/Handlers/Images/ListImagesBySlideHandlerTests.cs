using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Images;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Images;

public class ListImagesBySlideHandlerTests
{
    private const int ParentSlideId = 4;

    [Fact]
    public async Task HandleAsync_DelegatesToServiceFindBySlideAsyncAndReturnsMappedImages()
    {
        // Arrange
        var images = new List<Image>
        {
            new(ImageUrl.Create("/img.jpg"), ImageFileName.Create("img.jpg"), ImageOrder.Create(1), ParentSlideId)
        };
        var service = new Mock<IImageService>();
        service.Setup(s => s.FindBySlideAsync(ParentSlideId)).ReturnsAsync(images);

        // Act
        var result = await ListImagesBySlideHandler.HandleAsync(ParentSlideId, service.Object);

        // Assert
        Assert.Single(result.Value!);
    }
}

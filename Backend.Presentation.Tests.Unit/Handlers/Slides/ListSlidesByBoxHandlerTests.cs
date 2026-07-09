using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Slides;

public class ListSlidesByBoxHandlerTests
{
    private const int ParentBoxId = 11;

    [Fact]
    public async Task HandleAsync_DelegatesToServiceFindByBoxAsyncAndReturnsMappedSlides()
    {
        // Arrange
        var slides = new List<Slide> { new(SlideCode.Create("1TB07"), SlideName.Create("Gloecapsa"), ParentBoxId) };
        var service = new Mock<ISlideService>();
        service.Setup(s => s.FindByBoxAsync(ParentBoxId)).ReturnsAsync(slides);

        // Act
        var result = await ListSlidesByBoxHandler.HandleAsync(ParentBoxId, service.Object);

        // Assert
        Assert.Single(result.Value!);
    }
}

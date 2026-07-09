using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Slides;

public class ListSlidesHandlerTests
{
    private const int ParentBoxId = 11;
    private const int ValidPage = 1;
    private const int ValidPageSize = 20;
    private const int MaxPageSize = 100;

    private static List<Slide> OneSlide() =>
        [new(SlideCode.Create("1TB07"), SlideName.Create("Gloecapsa"), ParentBoxId)];

    [Fact]
    public async Task HandleAsync_ValidQuery_ReturnsOkWithPagedResponseEchoingPageAndPageSize()
    {
        // Arrange
        var service = new Mock<ISlideService>();
        service.Setup(s => s.SearchAsync(null, null, ValidPage, ValidPageSize)).ReturnsAsync((OneSlide(), 1));

        // Act
        var result = await ListSlidesHandler.HandleAsync(service.Object, null, null, ValidPage, ValidPageSize);

        // Assert
        var okResult = Assert.IsType<Ok<PagedSlidesResponse>>(result.Result);
        Assert.Equal(ValidPage, okResult.Value!.Page);
        Assert.Equal(ValidPageSize, okResult.Value!.PageSize);
        Assert.Equal(1, okResult.Value!.TotalCount);
    }

    [Fact]
    public async Task HandleAsync_PageBelowOne_ReturnsBadRequestWithPageError()
    {
        // Arrange
        var service = new Mock<ISlideService>();

        // Act
        var result = await ListSlidesHandler.HandleAsync(service.Object, null, null, 0, ValidPageSize);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        Assert.Contains(badRequestResult.Value!.Errors, e => e.Field == "Page");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MaxPageSize + 1)]
    public async Task HandleAsync_PageSizeOutsideRange_ReturnsBadRequestWithPageSizeError(int pageSize)
    {
        // Arrange
        var service = new Mock<ISlideService>();

        // Act
        var result = await ListSlidesHandler.HandleAsync(service.Object, null, null, ValidPage, pageSize);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        Assert.Contains(badRequestResult.Value!.Errors, e => e.Field == "PageSize");
    }

    [Fact]
    public async Task HandleAsync_PageAndPageSizeBothInvalid_ReturnsBadRequestWithBothErrors()
    {
        // Arrange
        var service = new Mock<ISlideService>();

        // Act
        var result = await ListSlidesHandler.HandleAsync(service.Object, null, null, 0, 0);

        // Assert
        var badRequestResult = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        Assert.Equal(2, badRequestResult.Value!.Errors.Count);
    }

    [Fact]
    public async Task HandleAsync_SearchAndBoxIdProvided_PassesThemToServiceSearchAsync()
    {
        // Arrange
        const string search = "Gloe";
        var service = new Mock<ISlideService>();
        service.Setup(s => s.SearchAsync(search, ParentBoxId, ValidPage, ValidPageSize)).ReturnsAsync((OneSlide(), 1));

        // Act
        await ListSlidesHandler.HandleAsync(service.Object, search, ParentBoxId, ValidPage, ValidPageSize);

        // Assert
        service.Verify(s => s.SearchAsync(search, ParentBoxId, ValidPage, ValidPageSize), Times.Once);
    }
}

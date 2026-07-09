using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Tests.Unit.Handlers.Boxes;

public class ListBoxesHandlerTests
{
    private const int ExistingId = 5;
    private const int ParentGroupId = 9;

    [Fact]
    public async Task HandleAsync_ReturnsOkWithAllMappedBoxes()
    {
        // Arrange
        var boxes = new List<Box> { new(ExistingId, BoxNumber.Create(7), BoxName.Create("Cyanobacteria"), ParentGroupId) };
        var service = new Mock<IBoxService>();
        service.Setup(s => s.ListAllAsync()).ReturnsAsync(boxes);

        // Act
        var result = await ListBoxesHandler.HandleAsync(service.Object);

        // Assert
        Assert.Single(result.Value!);
    }
}

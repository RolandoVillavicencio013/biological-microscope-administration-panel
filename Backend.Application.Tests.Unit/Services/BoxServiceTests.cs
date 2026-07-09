using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services.Implementations;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Tests.Unit.Services;

public class BoxServiceTests
{
    private const int ExistingId = 5;
    private const int ParentGroupId = 9;

    private static Box NewBox() => new(BoxNumber.Create(7), BoxName.Create("Cyanobacteria"), ParentGroupId);

    [Fact]
    public async Task CreateAsync_ValidBox_ReturnsRepositoryResult()
    {
        // Arrange
        var box = NewBox();
        var created = new Box(ExistingId, box.Number, box.Name, ParentGroupId);
        var repository = new Mock<IBoxRepository>();
        repository.Setup(r => r.CreateAsync(box)).ReturnsAsync(created);
        var service = new BoxService(repository.Object);

        // Act
        var result = await service.CreateAsync(box);

        // Assert
        Assert.Same(created, result);
    }

    [Fact]
    public async Task FindByIdAsync_ExistingId_ReturnsBoxFromRepository()
    {
        // Arrange
        var box = new Box(ExistingId, NewBox().Number, NewBox().Name, ParentGroupId);
        var repository = new Mock<IBoxRepository>();
        repository.Setup(r => r.ReadAsync(ExistingId)).ReturnsAsync(box);
        var service = new BoxService(repository.Object);

        // Act
        var result = await service.FindByIdAsync(ExistingId);

        // Assert
        Assert.Same(box, result);
    }

    [Fact]
    public async Task FindByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var repository = new Mock<IBoxRepository>();
        repository.Setup(r => r.ReadAsync(ExistingId)).ReturnsAsync((Box?)null);
        var service = new BoxService(repository.Object);

        // Act
        var result = await service.FindByIdAsync(ExistingId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ListAllAsync_ReturnsAllBoxesFromRepository()
    {
        // Arrange
        var boxes = new List<Box> { new(ExistingId, NewBox().Number, NewBox().Name, ParentGroupId) };
        var repository = new Mock<IBoxRepository>();
        repository.Setup(r => r.ListAsync()).ReturnsAsync(boxes);
        var service = new BoxService(repository.Object);

        // Act
        var result = await service.ListAllAsync();

        // Assert
        Assert.Same(boxes, result);
    }

    [Fact]
    public async Task FindByGroupAsync_DelegatesToRepositoryListByGroupAsync()
    {
        // Arrange
        var boxes = new List<Box> { new(ExistingId, NewBox().Number, NewBox().Name, ParentGroupId) };
        var repository = new Mock<IBoxRepository>();
        repository.Setup(r => r.ListByGroupAsync(ParentGroupId)).ReturnsAsync(boxes);
        var service = new BoxService(repository.Object);

        // Act
        var result = await service.FindByGroupAsync(ParentGroupId);

        // Assert
        Assert.Same(boxes, result);
    }

    [Fact]
    public async Task UpdateAsync_ValidBox_ReturnsRepositoryResult()
    {
        // Arrange
        var box = new Box(ExistingId, NewBox().Number, NewBox().Name, ParentGroupId);
        var repository = new Mock<IBoxRepository>();
        repository.Setup(r => r.UpdateAsync(box)).ReturnsAsync(box);
        var service = new BoxService(repository.Object);

        // Act
        var result = await service.UpdateAsync(box);

        // Assert
        Assert.Same(box, result);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_CallsRepositoryDeleteAsyncOnce()
    {
        // Arrange
        var repository = new Mock<IBoxRepository>();
        var service = new BoxService(repository.Object);

        // Act
        await service.DeleteAsync(ExistingId);

        // Assert
        repository.Verify(r => r.DeleteAsync(ExistingId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var box = NewBox();
        var repository = new Mock<IBoxRepository>();
        repository.Setup(r => r.CreateAsync(box)).ThrowsAsync(new InvalidOperationException("db failure"));
        var service = new BoxService(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(box));
    }
}

using Moq;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services.Implementations;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Tests.Unit.Services;

public class GroupServiceTests
{
    private const int ExistingId = 1;

    private static Group NewGroup() => new(GroupName.Create("Bacteria"));

    [Fact]
    public async Task CreateAsync_ValidGroup_ReturnsRepositoryResult()
    {
        // Arrange
        var group = NewGroup();
        var created = new Group(ExistingId, group.Name);
        var repository = new Mock<IGroupRepository>();
        repository.Setup(r => r.CreateAsync(group)).ReturnsAsync(created);
        var service = new GroupService(repository.Object);

        // Act
        var result = await service.CreateAsync(group);

        // Assert
        Assert.Same(created, result);
    }

    [Fact]
    public async Task FindByIdAsync_ExistingId_ReturnsGroupFromRepository()
    {
        // Arrange
        var group = new Group(ExistingId, NewGroup().Name);
        var repository = new Mock<IGroupRepository>();
        repository.Setup(r => r.ReadAsync(ExistingId)).ReturnsAsync(group);
        var service = new GroupService(repository.Object);

        // Act
        var result = await service.FindByIdAsync(ExistingId);

        // Assert
        Assert.Same(group, result);
    }

    [Fact]
    public async Task FindByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var repository = new Mock<IGroupRepository>();
        repository.Setup(r => r.ReadAsync(ExistingId)).ReturnsAsync((Group?)null);
        var service = new GroupService(repository.Object);

        // Act
        var result = await service.FindByIdAsync(ExistingId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ListAllAsync_ReturnsAllGroupsFromRepository()
    {
        // Arrange
        var groups = new List<Group> { new(ExistingId, NewGroup().Name) };
        var repository = new Mock<IGroupRepository>();
        repository.Setup(r => r.ListAsync()).ReturnsAsync(groups);
        var service = new GroupService(repository.Object);

        // Act
        var result = await service.ListAllAsync();

        // Assert
        Assert.Same(groups, result);
    }

    [Fact]
    public async Task UpdateAsync_ValidGroup_ReturnsRepositoryResult()
    {
        // Arrange
        var group = new Group(ExistingId, NewGroup().Name);
        var repository = new Mock<IGroupRepository>();
        repository.Setup(r => r.UpdateAsync(group)).ReturnsAsync(group);
        var service = new GroupService(repository.Object);

        // Act
        var result = await service.UpdateAsync(group);

        // Assert
        Assert.Same(group, result);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_CallsRepositoryDeleteAsyncOnce()
    {
        // Arrange
        var repository = new Mock<IGroupRepository>();
        var service = new GroupService(repository.Object);

        // Act
        await service.DeleteAsync(ExistingId);

        // Assert
        repository.Verify(r => r.DeleteAsync(ExistingId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var group = NewGroup();
        var repository = new Mock<IGroupRepository>();
        repository.Setup(r => r.CreateAsync(group)).ThrowsAsync(new InvalidOperationException("db failure"));
        var service = new GroupService(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(group));
    }
}

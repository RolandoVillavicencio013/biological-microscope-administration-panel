using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Tests.Unit.Repositories;

public class InMemoryGroupRepositoryTests
{
    private const string ValidName = "Bacteria";

    private static Group NewGroup(string name = ValidName) => new(GroupName.Create(name));

    [Fact]
    public async Task CreateAsync_TwoGroups_AssignsSequentialIds()
    {
        // Arrange
        var repository = new InMemoryGroupRepository();

        // Act
        var first = await repository.CreateAsync(NewGroup());
        var second = await repository.CreateAsync(NewGroup("Fungi"));

        // Assert
        Assert.Equal(1, first.Id);
        Assert.Equal(2, second.Id);
    }

    [Fact]
    public async Task ReadAsync_ExistingId_ReturnsPersistedGroup()
    {
        // Arrange
        var repository = new InMemoryGroupRepository();
        var created = await repository.CreateAsync(NewGroup());

        // Act
        var result = await repository.ReadAsync(created.Id);

        // Assert
        Assert.Equal(ValidName, result!.Name.Value);
    }

    [Fact]
    public async Task ReadAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var repository = new InMemoryGroupRepository();

        // Act
        var result = await repository.ReadAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ListAsync_ReturnsAllCreatedGroups()
    {
        // Arrange
        var repository = new InMemoryGroupRepository();
        await repository.CreateAsync(NewGroup());
        await repository.CreateAsync(NewGroup("Fungi"));

        // Act
        var result = await repository.ListAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UpdateAsync_ExistingGroup_ReplacesStoredName()
    {
        // Arrange
        var repository = new InMemoryGroupRepository();
        var created = await repository.CreateAsync(NewGroup());
        var updated = new Group(created.Id, GroupName.Create("Fungi"));

        // Act
        await repository.UpdateAsync(updated);
        var result = await repository.ReadAsync(created.Id);

        // Assert
        Assert.Equal("Fungi", result!.Name.Value);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var repository = new InMemoryGroupRepository();
        var group = new Group(1, GroupName.Create(ValidName));

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.UpdateAsync(group));
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_RemovesGroup()
    {
        // Arrange
        var repository = new InMemoryGroupRepository();
        var created = await repository.CreateAsync(NewGroup());

        // Act
        await repository.DeleteAsync(created.Id);
        var result = await repository.ReadAsync(created.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var repository = new InMemoryGroupRepository();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.DeleteAsync(1));
    }
}

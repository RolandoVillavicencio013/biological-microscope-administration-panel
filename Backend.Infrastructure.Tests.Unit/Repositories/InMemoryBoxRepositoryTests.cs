using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Tests.Unit.Repositories;

public class InMemoryBoxRepositoryTests
{
    private const int ParentGroupId = 9;
    private const int OtherGroupId = 10;

    private static Box NewBox(int groupId = ParentGroupId) =>
        new(BoxNumber.Create(7), BoxName.Create("Cyanobacteria"), groupId);

    [Fact]
    public async Task CreateAsync_TwoBoxes_AssignsSequentialIds()
    {
        // Arrange
        var repository = new InMemoryBoxRepository();

        // Act
        var first = await repository.CreateAsync(NewBox());
        var second = await repository.CreateAsync(NewBox());

        // Assert
        Assert.Equal(1, first.Id);
        Assert.Equal(2, second.Id);
    }

    [Fact]
    public async Task ReadAsync_ExistingId_ReturnsPersistedBox()
    {
        // Arrange
        var repository = new InMemoryBoxRepository();
        var created = await repository.CreateAsync(NewBox());

        // Act
        var result = await repository.ReadAsync(created.Id);

        // Assert
        Assert.Equal(ParentGroupId, result!.GroupId);
    }

    [Fact]
    public async Task ReadAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var repository = new InMemoryBoxRepository();

        // Act
        var result = await repository.ReadAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ListAsync_ReturnsAllCreatedBoxes()
    {
        // Arrange
        var repository = new InMemoryBoxRepository();
        await repository.CreateAsync(NewBox());
        await repository.CreateAsync(NewBox());

        // Act
        var result = await repository.ListAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ListByGroupAsync_ReturnsOnlyBoxesForRequestedGroup()
    {
        // Arrange
        var repository = new InMemoryBoxRepository();
        await repository.CreateAsync(NewBox(ParentGroupId));
        await repository.CreateAsync(NewBox(OtherGroupId));

        // Act
        var result = await repository.ListByGroupAsync(ParentGroupId);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateAsync_ExistingBox_ReplacesStoredValues()
    {
        // Arrange
        var repository = new InMemoryBoxRepository();
        var created = await repository.CreateAsync(NewBox());
        var updated = new Box(created.Id, BoxNumber.Create(8), BoxName.Create("Updated"), ParentGroupId);

        // Act
        await repository.UpdateAsync(updated);
        var result = await repository.ReadAsync(created.Id);

        // Assert
        Assert.Equal("Updated", result!.Name.Value);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var repository = new InMemoryBoxRepository();
        var box = new Box(1, BoxNumber.Create(7), BoxName.Create("Cyanobacteria"), ParentGroupId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.UpdateAsync(box));
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_RemovesBox()
    {
        // Arrange
        var repository = new InMemoryBoxRepository();
        var created = await repository.CreateAsync(NewBox());

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
        var repository = new InMemoryBoxRepository();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.DeleteAsync(1));
    }
}

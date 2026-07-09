using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Tests.Unit.Repositories;

public class EfBoxRepositoryTests
{
    private static Box NewBox(int groupId) =>
        new(BoxNumber.Create(7), BoxName.Create("Cyanobacteria"), groupId);

    private static async Task<int> SeedGroupAsync(AppDbContext context, string name = "Bacteria")
    {
        var group = new Group(GroupName.Create(name));
        context.Groups.Add(group);
        await context.SaveChangesAsync();
        return group.Id;
    }

    [Fact]
    public async Task CreateAsync_ValidBox_AssignsGeneratedId()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var groupId = await SeedGroupAsync(database.Context);
        var repository = new EfBoxRepository(database.Context);

        // Act
        var created = await repository.CreateAsync(NewBox(groupId));

        // Assert
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task ReadAsync_ExistingId_ReturnsPersistedBox()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var groupId = await SeedGroupAsync(database.Context);
        var repository = new EfBoxRepository(database.Context);
        var created = await repository.CreateAsync(NewBox(groupId));

        // Act
        var result = await repository.ReadAsync(created.Id);

        // Assert
        Assert.Equal(groupId, result!.GroupId);
    }

    [Fact]
    public async Task ReadAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var repository = new EfBoxRepository(database.Context);

        // Act
        var result = await repository.ReadAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ListAsync_ReturnsAllPersistedBoxes()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var groupId = await SeedGroupAsync(database.Context);
        var repository = new EfBoxRepository(database.Context);
        await repository.CreateAsync(NewBox(groupId));
        await repository.CreateAsync(NewBox(groupId));

        // Act
        var result = await repository.ListAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ListByGroupAsync_ReturnsOnlyBoxesForRequestedGroup()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var targetGroupId = await SeedGroupAsync(database.Context, "Bacteria");
        var otherGroupId = await SeedGroupAsync(database.Context, "Fungi");
        var repository = new EfBoxRepository(database.Context);
        await repository.CreateAsync(NewBox(targetGroupId));
        await repository.CreateAsync(NewBox(otherGroupId));

        // Act
        var result = await repository.ListByGroupAsync(targetGroupId);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateAsync_ExistingBox_PersistsNewValues()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var groupId = await SeedGroupAsync(database.Context);
        var created = await new EfBoxRepository(database.Context).CreateAsync(NewBox(groupId));
        var updated = new Box(created.Id, BoxNumber.Create(8), BoxName.Create("Updated"), groupId);

        // Act
        using var updateContext = database.CreateContext();
        await new EfBoxRepository(updateContext).UpdateAsync(updated);

        // Assert
        var result = await new EfBoxRepository(database.Context).ReadAsync(created.Id);
        Assert.Equal("Updated", result!.Name.Value);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_RemovesBox()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var groupId = await SeedGroupAsync(database.Context);
        var repository = new EfBoxRepository(database.Context);
        var created = await repository.CreateAsync(NewBox(groupId));

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
        using var database = new SqliteTestDatabase();
        var repository = new EfBoxRepository(database.Context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.DeleteAsync(1));
    }
}

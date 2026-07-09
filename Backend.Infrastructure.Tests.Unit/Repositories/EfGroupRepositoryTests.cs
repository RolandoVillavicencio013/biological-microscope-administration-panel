using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Tests.Unit.Repositories;

public class EfGroupRepositoryTests
{
    private const string ValidName = "Bacteria";

    private static Group NewGroup(string name = ValidName) => new(GroupName.Create(name));

    [Fact]
    public async Task CreateAsync_ValidGroup_AssignsGeneratedId()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var repository = new EfGroupRepository(database.Context);

        // Act
        var created = await repository.CreateAsync(NewGroup());

        // Assert
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task CreateAsync_ValidGroup_PersistsRetrievableGroup()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var repository = new EfGroupRepository(database.Context);
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
        using var database = new SqliteTestDatabase();
        var repository = new EfGroupRepository(database.Context);

        // Act
        var result = await repository.ReadAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ListAsync_ReturnsAllPersistedGroups()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var repository = new EfGroupRepository(database.Context);
        await repository.CreateAsync(NewGroup());
        await repository.CreateAsync(NewGroup("Fungi"));

        // Act
        var result = await repository.ListAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UpdateAsync_ExistingGroup_PersistsNewName()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var created = await new EfGroupRepository(database.Context).CreateAsync(NewGroup());
        var updated = new Group(created.Id, GroupName.Create("Fungi"));

        // Act
        using var updateContext = database.CreateContext();
        await new EfGroupRepository(updateContext).UpdateAsync(updated);

        // Assert
        var result = await new EfGroupRepository(database.Context).ReadAsync(created.Id);
        Assert.Equal("Fungi", result!.Name.Value);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_RemovesGroup()
    {
        // Arrange
        using var database = new SqliteTestDatabase();
        var repository = new EfGroupRepository(database.Context);
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
        using var database = new SqliteTestDatabase();
        var repository = new EfGroupRepository(database.Context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.DeleteAsync(1));
    }
}

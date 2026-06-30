using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;

/// <summary>Persistence contract for the <see cref="Group"/> entity.</summary>
public interface IGroupRepository
{
    /// <summary>Persists a new group and returns the entity with the DB-assigned id.</summary>
    Task<Group> CreateAsync(Group group);

    /// <summary>Returns the group with the given id, or null if not found.</summary>
    Task<Group?> ReadAsync(int id);

    /// <summary>Returns all registered groups.</summary>
    Task<List<Group>> ListAsync();

    /// <summary>Updates an existing group and returns the updated entity.</summary>
    Task<Group> UpdateAsync(Group group);

    /// <summary>Deletes the group with the given id.</summary>
    Task DeleteAsync(int id);
}

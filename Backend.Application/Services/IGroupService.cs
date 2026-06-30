using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;

/// <summary>Application service contract for <see cref="Group"/> use cases.</summary>
public interface IGroupService
{
    /// <summary>Creates a new group.</summary>
    Task<Group> CreateAsync(Group group);

    /// <summary>Returns the group with the given id, or null if not found.</summary>
    Task<Group?> FindByIdAsync(int id);

    /// <summary>Returns all registered groups.</summary>
    Task<List<Group>> ListAllAsync();

    /// <summary>Updates an existing group.</summary>
    Task<Group> UpdateAsync(Group group);

    /// <summary>Deletes the group with the given id.</summary>
    Task DeleteAsync(int id);
}

using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;

/// <summary>Persistence contract for the <see cref="Box"/> entity.</summary>
public interface IBoxRepository
{
    /// <summary>Persists a new box and returns the entity with the DB-assigned id.</summary>
    Task<Box> CreateAsync(Box box);

    /// <summary>Returns the box with the given id, or null if not found.</summary>
    Task<Box?> ReadAsync(int id);

    /// <summary>Returns all registered boxes.</summary>
    Task<List<Box>> ListAsync();

    /// <summary>Returns all boxes belonging to the given group.</summary>
    Task<List<Box>> ListByGroupAsync(int groupId);

    /// <summary>Updates an existing box and returns the updated entity.</summary>
    Task<Box> UpdateAsync(Box box);

    /// <summary>Deletes the box with the given id.</summary>
    Task DeleteAsync(int id);
}

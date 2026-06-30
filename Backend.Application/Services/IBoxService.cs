using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;

/// <summary>Application service contract for <see cref="Box"/> use cases.</summary>
public interface IBoxService
{
    /// <summary>Creates a new box.</summary>
    Task<Box> CreateAsync(Box box);

    /// <summary>Returns the box with the given id, or null if not found.</summary>
    Task<Box?> FindByIdAsync(int id);

    /// <summary>Returns all registered boxes.</summary>
    Task<List<Box>> ListAllAsync();

    /// <summary>Returns all boxes belonging to the given group.</summary>
    Task<List<Box>> FindByGroupAsync(int groupId);

    /// <summary>Updates an existing box.</summary>
    Task<Box> UpdateAsync(Box box);

    /// <summary>Deletes the box with the given id.</summary>
    Task DeleteAsync(int id);
}

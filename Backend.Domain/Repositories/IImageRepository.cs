using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;

/// <summary>Persistence contract for the <see cref="Image"/> entity.</summary>
public interface IImageRepository
{
    /// <summary>Persists a new image and returns the entity with the DB-assigned id.</summary>
    Task<Image> CreateAsync(Image image);

    /// <summary>Returns the image with the given id, or null if not found.</summary>
    Task<Image?> ReadAsync(int id);

    /// <summary>Returns all images for the given slide, ordered by <see cref="Image.Order"/>.</summary>
    Task<List<Image>> ListBySlideAsync(int slideId);

    /// <summary>Updates the order of an existing image and returns the updated entity.</summary>
    Task<Image> UpdateAsync(Image image);

    /// <summary>Deletes the image with the given id.</summary>
    Task DeleteAsync(int id);
}

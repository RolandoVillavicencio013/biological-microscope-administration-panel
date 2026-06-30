using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;

/// <summary>Application service contract for <see cref="Image"/> use cases.</summary>
public interface IImageService
{
    /// <summary>Creates a new image.</summary>
    Task<Image> CreateAsync(Image image);

    /// <summary>Returns the image with the given id, or null if not found.</summary>
    Task<Image?> FindByIdAsync(int id);

    /// <summary>Returns all images for the given slide, ordered by display position.</summary>
    Task<List<Image>> FindBySlideAsync(int slideId);

    /// <summary>Updates the order of an existing image.</summary>
    Task<Image> UpdateAsync(Image image);

    /// <summary>Deletes the image with the given id.</summary>
    Task DeleteAsync(int id);
}

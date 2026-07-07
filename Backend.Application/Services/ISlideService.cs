using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;

/// <summary>Application service contract for <see cref="Slide"/> use cases.</summary>
public interface ISlideService
{
    /// <summary>Creates a new slide.</summary>
    Task<Slide> CreateAsync(Slide slide);

    /// <summary>Returns the slide with the given id (including images), or null if not found.</summary>
    Task<Slide?> FindByIdAsync(int id);

    /// <summary>Returns all slides contained in the given box.</summary>
    Task<List<Slide>> FindByBoxAsync(int boxId);

    /// <summary>
    /// Returns a page of slides, optionally filtered by a case-insensitive match on code or
    /// name and/or by box, along with the total count of matching slides (ignoring paging).
    /// </summary>
    Task<(List<Slide> Items, int TotalCount)> SearchAsync(string? search, int? boxId, int page, int pageSize);

    /// <summary>Updates description, observations, and images of an existing slide.</summary>
    Task<Slide> UpdateAsync(Slide slide);

    /// <summary>Deletes the slide with the given id.</summary>
    Task DeleteAsync(int id);
}

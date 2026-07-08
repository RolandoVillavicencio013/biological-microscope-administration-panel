using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Services.Implementations;

/// <summary>Implements <see cref="ISlideService"/> by delegating to <see cref="ISlideRepository"/>.</summary>
internal class SlideService(ISlideRepository repository) : ISlideService
{
    public Task<Slide> CreateAsync(Slide slide) => repository.CreateAsync(slide);
    public Task<Slide?> FindByIdAsync(int id) => repository.ReadAsync(id);
    public Task<List<Slide>> FindByBoxAsync(int boxId) => repository.ListByBoxAsync(boxId);
    public Task<(List<Slide> Items, int TotalCount)> SearchAsync(string? search, int? boxId, int page, int pageSize) =>
        repository.SearchAsync(search, boxId, page, pageSize);
    public Task<Slide> UpdateAsync(Slide slide) => repository.UpdateAsync(slide);
    public Task DeleteAsync(int id) => repository.DeleteAsync(id);
}

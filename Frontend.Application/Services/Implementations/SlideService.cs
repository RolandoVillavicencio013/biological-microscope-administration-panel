using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Services.Implementations;

internal class SlideService(ISlideRepository repository) : ISlideService
{
    public Task<SlideDetailDto?> FindByIdAsync(int id) => repository.GetAsync(id);
    public Task<List<SlideDto>> ListByBoxAsync(int boxId) => repository.ListByBoxAsync(boxId);
    public Task<PagedSlidesDto> SearchAsync(string? search, int? boxId, int page, int pageSize) =>
        repository.SearchAsync(search, boxId, page, pageSize);
    public Task<SlideDto> CreateAsync(CreateSlideDto dto) => repository.CreateAsync(dto);
    public Task<SlideDto> UpdateAsync(int id, UpdateSlideDto dto) => repository.UpdateAsync(id, dto);
    public Task DeleteAsync(int id) => repository.DeleteAsync(id);
}

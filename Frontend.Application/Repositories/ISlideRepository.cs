using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Slides;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;

public interface ISlideRepository
{
    Task<SlideDetailDto?> GetAsync(int id);
    Task<List<SlideDto>> ListByBoxAsync(int boxId);
    Task<PagedSlidesDto> SearchAsync(string? search, int? boxId, int page, int pageSize);
    Task<SlideDto> CreateAsync(CreateSlideDto dto);
    Task<SlideDto> UpdateAsync(int id, UpdateSlideDto dto);
    Task DeleteAsync(int id);
}

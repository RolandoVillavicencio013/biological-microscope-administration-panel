using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Slides;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Services;

public interface ISlideService
{
    Task<List<SlideDto>> ListAllAsync();
    Task<SlideDetailDto?> FindByIdAsync(int id);
    Task<List<SlideDto>> ListByBoxAsync(int boxId);
    Task<SlideDto> CreateAsync(CreateSlideDto dto);
    Task<SlideDto> UpdateAsync(int id, UpdateSlideDto dto);
    Task DeleteAsync(int id);
}

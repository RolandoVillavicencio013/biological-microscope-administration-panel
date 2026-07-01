using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Boxes;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Services;

public interface IBoxService
{
    Task<List<BoxDto>> ListAllAsync();
    Task<BoxDto?> FindByIdAsync(int id);
    Task<List<BoxDto>> ListByGroupAsync(int groupId);
    Task<BoxDto> CreateAsync(CreateBoxDto dto);
    Task<BoxDto> UpdateAsync(int id, UpdateBoxDto dto);
    Task DeleteAsync(int id);
}

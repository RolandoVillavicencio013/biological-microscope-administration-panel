using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Boxes;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;

public interface IBoxRepository
{
    Task<List<BoxDto>> ListAsync();
    Task<BoxDto?> GetAsync(int id);
    Task<List<BoxDto>> ListByGroupAsync(int groupId);
    Task<BoxDto> CreateAsync(CreateBoxDto dto);
    Task<BoxDto> UpdateAsync(int id, UpdateBoxDto dto);
    Task DeleteAsync(int id);
}

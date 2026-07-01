using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Groups;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Services;

public interface IGroupService
{
    Task<List<GroupDto>> ListAllAsync();
    Task<GroupDto?> FindByIdAsync(int id);
    Task<GroupDto> CreateAsync(CreateGroupDto dto);
    Task<GroupDto> UpdateAsync(int id, CreateGroupDto dto);
    Task DeleteAsync(int id);
}

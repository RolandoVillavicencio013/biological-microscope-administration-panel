using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Groups;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;

public interface IGroupRepository
{
    Task<List<GroupDto>> ListAsync();
    Task<GroupDto?> GetAsync(int id);
    Task<GroupDto> CreateAsync(CreateGroupDto dto);
    Task<GroupDto> UpdateAsync(int id, CreateGroupDto dto);
    Task DeleteAsync(int id);
}

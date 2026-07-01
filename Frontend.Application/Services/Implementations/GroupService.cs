using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Groups;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Services.Implementations;

internal class GroupService(IGroupRepository repository) : IGroupService
{
    public Task<List<GroupDto>> ListAllAsync() => repository.ListAsync();
    public Task<GroupDto?> FindByIdAsync(int id) => repository.GetAsync(id);
    public Task<GroupDto> CreateAsync(CreateGroupDto dto) => repository.CreateAsync(dto);
    public Task<GroupDto> UpdateAsync(int id, CreateGroupDto dto) => repository.UpdateAsync(id, dto);
    public Task DeleteAsync(int id) => repository.DeleteAsync(id);
}

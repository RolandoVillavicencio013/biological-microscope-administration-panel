using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Groups;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;
using UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.ApiClient;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.Repositories;

internal sealed class GroupRepository(BioMicroscopeApiClient client) : IGroupRepository
{
    public async Task<List<GroupDto>> ListAsync()
    {
        var result = await client.GetGroupsAsync() ?? [];
        return result.Select(r => new GroupDto(r.Id, r.Name)).ToList();
    }

    public async Task<GroupDto?> GetAsync(int id)
    {
        var r = await client.GetGroupAsync(id);
        return r is null ? null : new GroupDto(r.Id, r.Name);
    }

    public async Task<GroupDto> CreateAsync(CreateGroupDto dto)
    {
        var r = await client.CreateGroupAsync(new CreateGroupRequest(dto.Name));
        return new GroupDto(r.Id, r.Name);
    }

    public async Task<GroupDto> UpdateAsync(int id, CreateGroupDto dto)
    {
        var r = await client.UpdateGroupAsync(id, new CreateGroupRequest(dto.Name));
        return new GroupDto(r.Id, r.Name);
    }

    public Task DeleteAsync(int id) => client.DeleteGroupAsync(id);
}

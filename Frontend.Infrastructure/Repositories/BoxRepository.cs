using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Boxes;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;
using UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.ApiClient;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.Repositories;

internal sealed class BoxRepository(BioMicroscopeApiClient client) : IBoxRepository
{
    private static BoxDto Map(BoxResponse r) => new(r.Id, r.Number, r.Name, r.GroupId);

    public async Task<List<BoxDto>> ListAsync()
    {
        var result = await client.GetBoxesAsync() ?? [];
        return result.Select(Map).ToList();
    }

    public async Task<BoxDto?> GetAsync(int id)
    {
        var r = await client.GetBoxAsync(id);
        return r is null ? null : Map(r);
    }

    public async Task<List<BoxDto>> ListByGroupAsync(int groupId)
    {
        var result = await client.GetBoxesByGroupAsync(groupId) ?? [];
        return result.Select(Map).ToList();
    }

    public async Task<BoxDto> CreateAsync(CreateBoxDto dto)
    {
        var r = await client.CreateBoxAsync(new CreateBoxRequest(dto.Number, dto.Name, dto.GroupId));
        return Map(r);
    }

    public async Task<BoxDto> UpdateAsync(int id, UpdateBoxDto dto)
    {
        var r = await client.UpdateBoxAsync(id, new UpdateBoxRequest(dto.Number, dto.Name));
        return Map(r);
    }

    public Task DeleteAsync(int id) => client.DeleteBoxAsync(id);
}

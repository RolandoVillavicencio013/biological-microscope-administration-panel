using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Boxes;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Services.Implementations;

internal class BoxService(IBoxRepository repository) : IBoxService
{
    public Task<List<BoxDto>> ListAllAsync() => repository.ListAsync();
    public Task<BoxDto?> FindByIdAsync(int id) => repository.GetAsync(id);
    public Task<List<BoxDto>> ListByGroupAsync(int groupId) => repository.ListByGroupAsync(groupId);
    public Task<BoxDto> CreateAsync(CreateBoxDto dto) => repository.CreateAsync(dto);
    public Task<BoxDto> UpdateAsync(int id, UpdateBoxDto dto) => repository.UpdateAsync(id, dto);
    public Task DeleteAsync(int id) => repository.DeleteAsync(id);
}

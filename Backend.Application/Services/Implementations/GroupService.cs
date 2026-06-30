using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Services.Implementations;

/// <summary>Implements <see cref="IGroupService"/> by delegating to <see cref="IGroupRepository"/>.</summary>
internal class GroupService(IGroupRepository repository) : IGroupService
{
    public Task<Group> CreateAsync(Group group) => repository.CreateAsync(group);
    public Task<Group?> FindByIdAsync(int id) => repository.ReadAsync(id);
    public Task<List<Group>> ListAllAsync() => repository.ListAsync();
    public Task<Group> UpdateAsync(Group group) => repository.UpdateAsync(group);
    public Task DeleteAsync(int id) => repository.DeleteAsync(id);
}

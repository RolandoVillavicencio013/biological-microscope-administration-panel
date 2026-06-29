using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Services.Implementations;

/// <summary>Implements <see cref="IBoxService"/> by delegating to <see cref="IBoxRepository"/>.</summary>
internal class BoxService(IBoxRepository repository) : IBoxService
{
    public Task<Box> CreateAsync(Box box) => repository.CreateAsync(box);
    public Task<Box?> FindByIdAsync(int id) => repository.ReadAsync(id);
    public Task<List<Box>> ListAllAsync() => repository.ListAsync();
    public Task<List<Box>> FindByGroupAsync(int groupId) => repository.ListByGroupAsync(groupId);
    public Task<Box> UpdateAsync(Box box) => repository.UpdateAsync(box);
    public Task DeleteAsync(int id) => repository.DeleteAsync(id);
}

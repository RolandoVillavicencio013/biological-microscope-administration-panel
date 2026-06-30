using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

/// <summary>In-memory implementation of <see cref="IGroupRepository"/>. Intended for development only.</summary>
internal class InMemoryGroupRepository : IGroupRepository
{
    private readonly List<Group> _groups = [];
    private int _nextId = 0;

    public Task<Group> CreateAsync(Group group)
    {
        var created = new Group(++_nextId, group.Name);
        _groups.Add(created);
        return Task.FromResult(created);
    }

    public Task<Group?> ReadAsync(int id) =>
        Task.FromResult(_groups.FirstOrDefault(g => g.Id == id));

    public Task<List<Group>> ListAsync() =>
        Task.FromResult(_groups.ToList());

    public Task<Group> UpdateAsync(Group group)
    {
        var index = _groups.FindIndex(g => g.Id == group.Id);
        if (index < 0) throw new NotFoundException($"Group {group.Id} not found.");
        _groups[index] = group;
        return Task.FromResult(group);
    }

    public Task DeleteAsync(int id)
    {
        var removed = _groups.RemoveAll(g => g.Id == id);
        if (removed == 0) throw new NotFoundException($"Group {id} not found.");
        return Task.CompletedTask;
    }
}

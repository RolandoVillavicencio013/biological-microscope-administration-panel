using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

/// <summary>In-memory implementation of <see cref="IBoxRepository"/>. Intended for development only.</summary>
internal class InMemoryBoxRepository : IBoxRepository
{
    private readonly List<Box> _boxes = [];
    private int _nextId = 0;

    public Task<Box> CreateAsync(Box box)
    {
        var created = new Box(++_nextId, box.Number, box.Name, box.GroupId);
        _boxes.Add(created);
        return Task.FromResult(created);
    }

    public Task<Box?> ReadAsync(int id) =>
        Task.FromResult(_boxes.FirstOrDefault(b => b.Id == id));

    public Task<List<Box>> ListAsync() =>
        Task.FromResult(_boxes.ToList());

    public Task<List<Box>> ListByGroupAsync(int groupId) =>
        Task.FromResult(_boxes.Where(b => b.GroupId == groupId).ToList());

    public Task<Box> UpdateAsync(Box box)
    {
        var index = _boxes.FindIndex(b => b.Id == box.Id);
        if (index < 0) throw new NotFoundException($"Box {box.Id} not found.");
        _boxes[index] = box;
        return Task.FromResult(box);
    }

    public Task DeleteAsync(int id)
    {
        var removed = _boxes.RemoveAll(b => b.Id == id);
        if (removed == 0) throw new NotFoundException($"Box {id} not found.");
        return Task.CompletedTask;
    }
}

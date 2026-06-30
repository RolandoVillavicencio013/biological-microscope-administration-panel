using Microsoft.EntityFrameworkCore;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

internal sealed class EfBoxRepository(AppDbContext context) : IBoxRepository
{
    public async Task<Box> CreateAsync(Box box)
    {
        context.Boxes.Add(box);
        await context.SaveChangesAsync();
        return box;
    }

    public Task<Box?> ReadAsync(int id) =>
        context.Boxes.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

    public Task<List<Box>> ListAsync() =>
        context.Boxes.AsNoTracking().ToListAsync();

    public Task<List<Box>> ListByGroupAsync(int groupId) =>
        context.Boxes.AsNoTracking()
            .Where(b => b.GroupId == groupId)
            .ToListAsync();

    public async Task<Box> UpdateAsync(Box box)
    {
        context.Boxes.Update(box);
        await context.SaveChangesAsync();
        return box;
    }

    public async Task DeleteAsync(int id)
    {
        var deleted = await context.Boxes.Where(b => b.Id == id).ExecuteDeleteAsync();
        if (deleted == 0) throw new NotFoundException($"Box {id} not found.");
    }
}

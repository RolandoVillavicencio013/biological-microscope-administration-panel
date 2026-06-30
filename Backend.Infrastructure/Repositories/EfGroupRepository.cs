using Microsoft.EntityFrameworkCore;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

internal sealed class EfGroupRepository(AppDbContext context) : IGroupRepository
{
    public async Task<Group> CreateAsync(Group group)
    {
        context.Groups.Add(group);
        await context.SaveChangesAsync();
        return group;
    }

    public Task<Group?> ReadAsync(int id) =>
        context.Groups.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);

    public Task<List<Group>> ListAsync() =>
        context.Groups.AsNoTracking().ToListAsync();

    public async Task<Group> UpdateAsync(Group group)
    {
        context.Groups.Update(group);
        await context.SaveChangesAsync();
        return group;
    }

    public async Task DeleteAsync(int id)
    {
        var deleted = await context.Groups.Where(g => g.Id == id).ExecuteDeleteAsync();
        if (deleted == 0) throw new NotFoundException($"Group {id} not found.");
    }
}

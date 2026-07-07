using Microsoft.EntityFrameworkCore;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

internal sealed class EfSlideRepository(AppDbContext context) : ISlideRepository
{
    public async Task<Slide> CreateAsync(Slide slide)
    {
        context.Slides.Add(slide);
        await context.SaveChangesAsync();
        return slide;
    }

    public Task<Slide?> ReadAsync(int id) =>
        context.Slides
            .AsNoTracking()
            .Include(s => s.Images)
            .FirstOrDefaultAsync(s => s.Id == id);

    public Task<List<Slide>> ListByBoxAsync(int boxId) =>
        context.Slides.AsNoTracking()
            .Where(s => s.BoxId == boxId)
            .ToListAsync();

    public async Task<(List<Slide> Items, int TotalCount)> SearchAsync(
        string? search, int? boxId, int page, int pageSize)
    {
        // Code/Name are mapped through a ValueConverter, so EF Core cannot translate member
        // access into the wrapped string (e.g. s.Code.Value) for a SQL ILIKE. BoxId narrows the
        // scan server-side; the text search then runs in memory over that already-small set.
        var query = context.Slides.AsNoTracking();
        if (boxId is not null)
            query = query.Where(s => s.BoxId == boxId);

        var candidates = await query.ToListAsync();

        var matched = string.IsNullOrWhiteSpace(search)
            ? candidates
            : candidates
                .Where(s =>
                    s.Code.Value.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    s.Name.Value.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

        var items = matched
            .OrderBy(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, matched.Count);
    }

    public async Task<Slide> UpdateAsync(Slide slide)
    {
        // Load a tracked instance and apply only the mutable fields to avoid
        // cascading Updates onto the Images collection.
        var tracked = await context.Slides.FindAsync(slide.Id)
            ?? throw new NotFoundException($"Slide {slide.Id} not found.");
        tracked.Description = slide.Description;
        tracked.Observations = slide.Observations;
        await context.SaveChangesAsync();
        return tracked;
    }

    public async Task DeleteAsync(int id)
    {
        var deleted = await context.Slides.Where(s => s.Id == id).ExecuteDeleteAsync();
        if (deleted == 0) throw new NotFoundException($"Slide {id} not found.");
    }
}

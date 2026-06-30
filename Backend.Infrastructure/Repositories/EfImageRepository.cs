using Microsoft.EntityFrameworkCore;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

internal sealed class EfImageRepository(AppDbContext context) : IImageRepository
{
    public async Task<Image> CreateAsync(Image image)
    {
        context.Images.Add(image);
        await context.SaveChangesAsync();
        return image;
    }

    public Task<Image?> ReadAsync(int id) =>
        context.Images.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

    public Task<List<Image>> ListBySlideAsync(int slideId) =>
        context.Images.AsNoTracking()
            .Where(i => i.SlideId == slideId)
            .OrderBy(i => i.Order)
            .ToListAsync();

    public async Task<Image> UpdateAsync(Image image)
    {
        // Load a tracked instance and apply only Order (the sole mutable field).
        var tracked = await context.Images.FindAsync(image.Id)
            ?? throw new NotFoundException($"Image {image.Id} not found.");
        tracked.Order = image.Order;
        await context.SaveChangesAsync();
        return tracked;
    }

    public async Task DeleteAsync(int id)
    {
        var deleted = await context.Images.Where(i => i.Id == id).ExecuteDeleteAsync();
        if (deleted == 0) throw new NotFoundException($"Image {id} not found.");
    }
}

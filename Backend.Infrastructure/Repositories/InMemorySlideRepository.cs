using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

/// <summary>In-memory implementation of <see cref="ISlideRepository"/>. Intended for development only.</summary>
internal class InMemorySlideRepository : ISlideRepository
{
    private readonly List<Slide> _slides = [];
    private int _nextId = 0;

    public Task<Slide> CreateAsync(Slide slide)
    {
        var created = new Slide(++_nextId, slide.Code, slide.Name, slide.BoxId,
            slide.Description, slide.Observations);
        _slides.Add(created);
        return Task.FromResult(created);
    }

    public Task<Slide?> ReadAsync(int id) =>
        Task.FromResult(_slides.FirstOrDefault(s => s.Id == id));

    public Task<List<Slide>> ListByBoxAsync(int boxId) =>
        Task.FromResult(_slides.Where(s => s.BoxId == boxId).ToList());

    public Task<(List<Slide> Items, int TotalCount)> SearchAsync(
        string? search, int? boxId, int page, int pageSize)
    {
        IEnumerable<Slide> query = _slides;

        if (boxId is not null)
            query = query.Where(s => s.BoxId == boxId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s =>
                s.Code.Value.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.Name.Value.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var matched = query.ToList();
        var items = matched.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return Task.FromResult((items, matched.Count));
    }

    public Task<Slide> UpdateAsync(Slide slide)
    {
        var index = _slides.FindIndex(s => s.Id == slide.Id);
        if (index < 0) throw new NotFoundException($"Slide {slide.Id} not found.");
        _slides[index] = slide;
        return Task.FromResult(slide);
    }

    public Task DeleteAsync(int id)
    {
        var removed = _slides.RemoveAll(s => s.Id == id);
        if (removed == 0) throw new NotFoundException($"Slide {id} not found.");
        return Task.CompletedTask;
    }
}

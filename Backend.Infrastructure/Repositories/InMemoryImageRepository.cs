using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Repositories;

/// <summary>In-memory implementation of <see cref="IImageRepository"/>. Intended for development only.</summary>
internal class InMemoryImageRepository : IImageRepository
{
    private readonly List<Image> _images = [];
    private int _nextId = 0;

    public Task<Image> CreateAsync(Image image)
    {
        var created = new Image(++_nextId, image.FileUrl, image.FileName, image.Order,
            image.UploadedAt, image.SlideId);
        _images.Add(created);
        return Task.FromResult(created);
    }

    public Task<Image?> ReadAsync(int id) =>
        Task.FromResult(_images.FirstOrDefault(i => i.Id == id));

    public Task<List<Image>> ListBySlideAsync(int slideId) =>
        Task.FromResult(_images.Where(i => i.SlideId == slideId)
            .OrderBy(i => i.Order.Value)
            .ToList());

    public Task<Image> UpdateAsync(Image image)
    {
        var index = _images.FindIndex(i => i.Id == image.Id);
        if (index < 0) throw new NotFoundException($"Image {image.Id} not found.");
        _images[index] = image;
        return Task.FromResult(image);
    }

    public Task DeleteAsync(int id)
    {
        var removed = _images.RemoveAll(i => i.Id == id);
        if (removed == 0) throw new NotFoundException($"Image {id} not found.");
        return Task.CompletedTask;
    }
}

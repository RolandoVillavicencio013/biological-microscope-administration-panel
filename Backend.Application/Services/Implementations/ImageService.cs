using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Application.Services.Implementations;

/// <summary>Implements <see cref="IImageService"/> by delegating to <see cref="IImageRepository"/>.</summary>
internal class ImageService(IImageRepository repository) : IImageService
{
    public Task<Image> CreateAsync(Image image) => repository.CreateAsync(image);
    public Task<Image?> FindByIdAsync(int id) => repository.ReadAsync(id);
    public Task<List<Image>> FindBySlideAsync(int slideId) => repository.ListBySlideAsync(slideId);
    public Task<Image> UpdateAsync(Image image) => repository.UpdateAsync(image);
    public Task DeleteAsync(int id) => repository.DeleteAsync(id);
}

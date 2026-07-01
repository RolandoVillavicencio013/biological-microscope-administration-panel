using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Images;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Services.Implementations;

internal class ImageService(IImageRepository repository) : IImageService
{
    public Task<List<ImageDto>> ListBySlideAsync(int slideId) => repository.ListBySlideAsync(slideId);
    public Task<ImageDto> UploadAsync(int slideId, Stream fileStream, string fileName, int order) =>
        repository.UploadAsync(slideId, fileStream, fileName, order);
    public Task<ImageDto> UpdateOrderAsync(int id, UpdateImageOrderDto dto) => repository.UpdateOrderAsync(id, dto);
    public Task DeleteAsync(int id) => repository.DeleteAsync(id);
}

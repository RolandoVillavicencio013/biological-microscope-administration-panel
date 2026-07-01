using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Images;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Services;

public interface IImageService
{
    Task<List<ImageDto>> ListBySlideAsync(int slideId);
    Task<ImageDto> UploadAsync(int slideId, Stream fileStream, string fileName, int order);
    Task<ImageDto> UpdateOrderAsync(int id, UpdateImageOrderDto dto);
    Task DeleteAsync(int id);
}

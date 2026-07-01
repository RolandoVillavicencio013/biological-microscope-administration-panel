using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Images;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;
using UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.ApiClient;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.Repositories;

internal sealed class ImageRepository(BioMicroscopeApiClient client) : IImageRepository
{
    private static ImageDto Map(ImageResponse i) =>
        new(i.Id, i.FileUrl, i.FileName, i.Order, i.UploadedAt, i.SlideId);

    public async Task<List<ImageDto>> ListBySlideAsync(int slideId)
    {
        var result = await client.GetImagesAsync(slideId) ?? [];
        return result.Select(Map).ToList();
    }

    public async Task<ImageDto> UploadAsync(int slideId, Stream fileStream, string fileName, int order)
    {
        var r = await client.UploadImageAsync(slideId, fileStream, fileName, order);
        return Map(r);
    }

    public async Task<ImageDto> UpdateOrderAsync(int id, UpdateImageOrderDto dto)
    {
        var r = await client.UpdateImageOrderAsync(id, new UpdateImageOrderRequest(dto.Order));
        return Map(r);
    }

    public Task DeleteAsync(int id) => client.DeleteImageAsync(id);
}

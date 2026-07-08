using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Images;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Repositories;
using UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.ApiClient;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.Repositories;

internal sealed class SlideRepository(BioMicroscopeApiClient client) : ISlideRepository
{
    private static SlideDto Map(SlideResponse r) =>
        new(r.Id, r.Code, r.Name, r.BoxId, r.Description, r.Observations);

    public async Task<SlideDetailDto?> GetAsync(int id)
    {
        var r = await client.GetSlideAsync(id);
        if (r is null) return null;
        var images = r.Images.Select(i =>
            new ImageDto(i.Id, i.FileUrl, i.FileName, i.Order, i.UploadedAt, i.SlideId)).ToList();
        return new SlideDetailDto(r.Id, r.Code, r.Name, r.BoxId, r.Description, r.Observations, images);
    }

    public async Task<List<SlideDto>> ListByBoxAsync(int boxId)
    {
        var result = await client.GetSlidesByBoxAsync(boxId) ?? [];
        return result.Select(Map).ToList();
    }

    public async Task<PagedSlidesDto> SearchAsync(string? search, int? boxId, int page, int pageSize)
    {
        var result = await client.GetSlidesAsync(search, boxId, page, pageSize)
            ?? new PagedSlidesResponse([], 0, page, pageSize);
        return new PagedSlidesDto(result.Items.Select(Map).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<SlideDto> CreateAsync(CreateSlideDto dto)
    {
        var r = await client.CreateSlideAsync(
            new CreateSlideRequest(dto.Code, dto.Name, dto.BoxId, dto.Description, dto.Observations));
        return Map(r);
    }

    public async Task<SlideDto> UpdateAsync(int id, UpdateSlideDto dto)
    {
        var r = await client.UpdateSlideAsync(id, new UpdateSlideRequest(dto.Description, dto.Observations));
        return Map(r);
    }

    public Task DeleteAsync(int id) => client.DeleteSlideAsync(id);
}

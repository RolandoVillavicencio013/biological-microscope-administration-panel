using UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Images;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Slides;

public record SlideDto(int Id, string Code, string Name, int BoxId, string? Description, string? Observations);
public record PagedSlidesDto(List<SlideDto> Items, int TotalCount, int Page, int PageSize);
public record SlideDetailDto(int Id, string Code, string Name, int BoxId, string? Description, string? Observations, List<ImageDto> Images);
public record CreateSlideDto(string Code, string Name, int BoxId, string? Description, string? Observations);
public record UpdateSlideDto(string? Description, string? Observations);

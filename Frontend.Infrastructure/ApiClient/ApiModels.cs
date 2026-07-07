namespace UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.ApiClient;

// These mirror the backend's Presentation DTOs exactly.
internal record GroupResponse(int Id, string Name);
internal record BoxResponse(int Id, int Number, string Name, int GroupId);
internal record SlideResponse(int Id, string Code, string Name, int BoxId, string? Description, string? Observations);
internal record SlideDetailResponse(int Id, string Code, string Name, int BoxId, string? Description, string? Observations, List<ImageResponse> Images);
internal record PagedSlidesResponse(List<SlideResponse> Items, int TotalCount, int Page, int PageSize);
internal record ImageResponse(int Id, string FileUrl, string FileName, int Order, DateTime UploadedAt, int SlideId);

// Request bodies sent to the backend.
internal record CreateGroupRequest(string Name);
internal record CreateBoxRequest(int Number, string Name, int GroupId);
internal record UpdateBoxRequest(int Number, string Name);
internal record CreateSlideRequest(string Code, string Name, int BoxId, string? Description, string? Observations);
internal record UpdateSlideRequest(string? Description, string? Observations);
internal record UpdateImageOrderRequest(int Order);

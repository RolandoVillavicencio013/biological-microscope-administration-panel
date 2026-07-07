namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;

/// <summary>Paged response body for GET /slides.</summary>
public record PagedSlidesResponse(List<SlideResponse> Items, int TotalCount, int Page, int PageSize);

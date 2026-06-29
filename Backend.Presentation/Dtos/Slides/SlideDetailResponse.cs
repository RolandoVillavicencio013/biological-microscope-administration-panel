using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Images;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;

/// <summary>Response body representing a slide with its associated images (for detail view).</summary>
public record SlideDetailResponse(
    int Id,
    string Code,
    string Name,
    int BoxId,
    string? Description,
    string? Observations,
    List<ImageResponse> Images);

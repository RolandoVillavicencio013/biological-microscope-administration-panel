namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;

/// <summary>Response body representing a slide (without images, for list views).</summary>
public record SlideResponse(int Id, string Code, string Name, int BoxId, string? Description, string? Observations);

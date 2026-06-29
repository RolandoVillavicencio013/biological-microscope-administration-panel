namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;

/// <summary>Request body for creating a new slide.</summary>
public record CreateSlideRequest(string Code, string Name, int BoxId, string? Description, string? Observations);

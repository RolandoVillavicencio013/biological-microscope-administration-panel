namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;

/// <summary>Request body for updating a slide's editable fields (RF-3).</summary>
public record UpdateSlideRequest(string? Description, string? Observations);

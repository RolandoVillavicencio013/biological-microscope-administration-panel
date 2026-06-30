namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Images;

/// <summary>Response body representing an image associated with a slide.</summary>
public record ImageResponse(int Id, string FileUrl, string FileName, int Order, DateTime UploadedAt, int SlideId);

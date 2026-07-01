namespace UCR.EB.BioMicroscopeAdmin.Frontend.Application.Dtos.Images;

public record ImageDto(int Id, string FileUrl, string FileName, int Order, DateTime UploadedAt, int SlideId);
public record UpdateImageOrderDto(int Order);

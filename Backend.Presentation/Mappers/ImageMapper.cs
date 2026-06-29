using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Images;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

/// <summary>Maps between <see cref="Image"/> entities and their DTOs.</summary>
public static class ImageMapper
{
    /// <summary>Converts an <see cref="Image"/> entity to an <see cref="ImageResponse"/>.</summary>
    public static ImageResponse ToResponse(Image image) =>
        new(image.Id, image.FileUrl.Value, image.FileName.Value,
            image.Order.Value, image.UploadedAt, image.SlideId);

    /// <summary>Applies an updated order to an existing <see cref="Image"/> entity.</summary>
    public static void ApplyOrderUpdate(Image image, int order)
    {
        if (!ImageOrder.TryCreate(order, out var imageOrder))
            throw new ValidationException($"Order must be between {ImageOrder.Min} and {ImageOrder.Max}.");

        image.Order = imageOrder!;
    }
}

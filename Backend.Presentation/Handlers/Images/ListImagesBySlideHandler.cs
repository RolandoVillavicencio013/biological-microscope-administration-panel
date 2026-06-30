using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Images;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Images;

/// <summary>Handles GET /slides/{id}/images — returns all images for a slide ordered by position.</summary>
public static class ListImagesBySlideHandler
{
    public static async Task<Ok<List<ImageResponse>>> HandleAsync(
        [FromRoute] int id,
        [FromServices] IImageService service)
    {
        var images = await service.FindBySlideAsync(id);
        return TypedResults.Ok(images.Select(ImageMapper.ToResponse).ToList());
    }
}

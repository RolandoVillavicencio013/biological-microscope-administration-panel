using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

/// <summary>Handles GET /slides — returns all slides.</summary>
public static class ListSlidesHandler
{
    public static async Task<Ok<List<SlideResponse>>> HandleAsync(
        [FromServices] ISlideService service)
    {
        var slides = await service.ListAllAsync();
        return TypedResults.Ok(slides.Select(SlideMapper.ToResponse).ToList());
    }
}

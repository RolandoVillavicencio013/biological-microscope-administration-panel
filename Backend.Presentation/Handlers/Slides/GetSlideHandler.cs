using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

/// <summary>Handles GET /slides/{id} — returns a slide with its images.</summary>
public static class GetSlideHandler
{
    public static async Task<Results<Ok<SlideDetailResponse>, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        [FromServices] ISlideService service)
    {
        var slide = await service.FindByIdAsync(id);
        return slide is null
            ? TypedResults.NotFound($"Slide {id} not found.")
            : TypedResults.Ok(SlideMapper.ToDetailResponse(slide));
    }
}

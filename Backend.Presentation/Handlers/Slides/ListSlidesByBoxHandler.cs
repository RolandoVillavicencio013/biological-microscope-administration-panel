using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

/// <summary>Handles GET /boxes/{id}/slides — returns all slides in a box.</summary>
public static class ListSlidesByBoxHandler
{
    public static async Task<Ok<List<SlideResponse>>> HandleAsync(
        [FromRoute] int id,
        [FromServices] ISlideService service)
    {
        var slides = await service.FindByBoxAsync(id);
        return TypedResults.Ok(slides.Select(SlideMapper.ToResponse).ToList());
    }
}

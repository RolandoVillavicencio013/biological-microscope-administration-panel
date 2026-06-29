using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

/// <summary>Handles DELETE /slides/{id} — deletes a slide.</summary>
public static class DeleteSlideHandler
{
    public static async Task<Results<NoContent, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        [FromServices] ISlideService service)
    {
        var existing = await service.FindByIdAsync(id);
        if (existing is null) return TypedResults.NotFound($"Slide {id} not found.");

        await service.DeleteAsync(id);
        return TypedResults.NoContent();
    }
}

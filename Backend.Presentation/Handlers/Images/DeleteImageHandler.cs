using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Images;

/// <summary>Handles DELETE /images/{id} — deletes an image.</summary>
public static class DeleteImageHandler
{
    public static async Task<Results<NoContent, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        [FromServices] IImageService service)
    {
        var existing = await service.FindByIdAsync(id);
        if (existing is null) return TypedResults.NotFound($"Image {id} not found.");

        await service.DeleteAsync(id);
        return TypedResults.NoContent();
    }
}

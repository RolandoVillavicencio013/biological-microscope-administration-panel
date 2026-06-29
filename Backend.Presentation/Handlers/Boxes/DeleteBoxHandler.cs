using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

/// <summary>Handles DELETE /boxes/{id} — deletes a box.</summary>
public static class DeleteBoxHandler
{
    public static async Task<Results<NoContent, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        [FromServices] IBoxService service)
    {
        var existing = await service.FindByIdAsync(id);
        if (existing is null) return TypedResults.NotFound($"Box {id} not found.");

        await service.DeleteAsync(id);
        return TypedResults.NoContent();
    }
}

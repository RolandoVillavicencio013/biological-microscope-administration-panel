using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Groups;

/// <summary>Handles DELETE /groups/{id} — deletes a group.</summary>
public static class DeleteGroupHandler
{
    public static async Task<Results<NoContent, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        [FromServices] IGroupService service)
    {
        var existing = await service.FindByIdAsync(id);
        if (existing is null) return TypedResults.NotFound($"Group {id} not found.");

        await service.DeleteAsync(id);
        return TypedResults.NoContent();
    }
}

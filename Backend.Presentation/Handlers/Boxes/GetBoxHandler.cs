using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

/// <summary>Handles GET /boxes/{id} — returns a single box by id.</summary>
public static class GetBoxHandler
{
    public static async Task<Results<Ok<BoxResponse>, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        [FromServices] IBoxService service)
    {
        var box = await service.FindByIdAsync(id);
        return box is null
            ? TypedResults.NotFound($"Box {id} not found.")
            : TypedResults.Ok(BoxMapper.ToResponse(box));
    }
}

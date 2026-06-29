using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

/// <summary>Handles GET /groups/{id}/boxes — returns all boxes belonging to a group.</summary>
public static class ListBoxesByGroupHandler
{
    public static async Task<Ok<List<BoxResponse>>> HandleAsync(
        [FromRoute] int id,
        [FromServices] IBoxService service)
    {
        var boxes = await service.FindByGroupAsync(id);
        return TypedResults.Ok(boxes.Select(BoxMapper.ToResponse).ToList());
    }
}

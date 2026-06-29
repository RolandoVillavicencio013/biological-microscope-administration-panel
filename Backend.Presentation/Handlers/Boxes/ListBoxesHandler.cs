using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

/// <summary>Handles GET /boxes — returns all boxes.</summary>
public static class ListBoxesHandler
{
    public static async Task<Ok<List<BoxResponse>>> HandleAsync(
        [FromServices] IBoxService service)
    {
        var boxes = await service.ListAllAsync();
        return TypedResults.Ok(boxes.Select(BoxMapper.ToResponse).ToList());
    }
}

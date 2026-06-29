using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

/// <summary>Handles PUT /boxes/{id} — updates a box's number and name.</summary>
public static class UpdateBoxHandler
{
    public static async Task<Results<Ok<BoxResponse>, BadRequest<ErrorResponse>, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        [FromBody] UpdateBoxRequest request,
        [FromServices] IBoxService service)
    {
        try
        {
            var existing = await service.FindByIdAsync(id);
            if (existing is null) return TypedResults.NotFound($"Box {id} not found.");

            var updated = BoxMapper.ToUpdatedEntity(id, existing.GroupId, request);
            var result = await service.UpdateAsync(updated);
            return TypedResults.Ok(BoxMapper.ToResponse(result));
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(new ErrorResponse(ex.Errors));
        }
    }
}

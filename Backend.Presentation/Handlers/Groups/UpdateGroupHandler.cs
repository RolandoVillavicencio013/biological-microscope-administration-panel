using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Groups;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Groups;

/// <summary>Handles PUT /groups/{id} — updates a group's name.</summary>
public static class UpdateGroupHandler
{
    public static async Task<Results<Ok<GroupResponse>, BadRequest<ErrorResponse>, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        [FromBody] CreateGroupRequest request,
        [FromServices] IGroupService service)
    {
        try
        {
            var existing = await service.FindByIdAsync(id);
            if (existing is null) return TypedResults.NotFound($"Group {id} not found.");

            var updated = GroupMapper.ToUpdatedEntity(id, request);
            var result = await service.UpdateAsync(updated);
            return TypedResults.Ok(GroupMapper.ToResponse(result));
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(new ErrorResponse(ex.Errors));
        }
    }
}

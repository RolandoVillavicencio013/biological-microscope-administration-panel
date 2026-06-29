using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Groups;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Groups;

/// <summary>Handles POST /groups — creates a new biological group.</summary>
public static class CreateGroupHandler
{
    public static async Task<Results<Created<GroupResponse>, BadRequest<ErrorResponse>>> HandleAsync(
        [FromBody] CreateGroupRequest request,
        [FromServices] IGroupService service)
    {
        try
        {
            var entity = GroupMapper.ToEntity(request);
            var created = await service.CreateAsync(entity);
            return TypedResults.Created($"/groups/{created.Id}", GroupMapper.ToResponse(created));
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(new ErrorResponse(ex.Errors));
        }
    }
}

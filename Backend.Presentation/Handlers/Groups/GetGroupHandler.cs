using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Groups;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Groups;

/// <summary>Handles GET /groups/{id} — returns a single group by id.</summary>
public static class GetGroupHandler
{
    public static async Task<Results<Ok<GroupResponse>, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        [FromServices] IGroupService service)
    {
        var group = await service.FindByIdAsync(id);
        return group is null
            ? TypedResults.NotFound($"Group {id} not found.")
            : TypedResults.Ok(GroupMapper.ToResponse(group));
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Groups;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Groups;

/// <summary>Handles GET /groups — returns all groups.</summary>
public static class ListGroupsHandler
{
    public static async Task<Ok<List<GroupResponse>>> HandleAsync(
        [FromServices] IGroupService service)
    {
        var groups = await service.ListAllAsync();
        return TypedResults.Ok(groups.Select(GroupMapper.ToResponse).ToList());
    }
}

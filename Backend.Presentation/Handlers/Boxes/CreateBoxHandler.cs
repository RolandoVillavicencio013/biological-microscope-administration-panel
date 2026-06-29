using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Boxes;

/// <summary>Handles POST /boxes — creates a new box.</summary>
public static class CreateBoxHandler
{
    public static async Task<Results<Created<BoxResponse>, BadRequest<ErrorResponse>>> HandleAsync(
        [FromBody] CreateBoxRequest request,
        [FromServices] IBoxService service)
    {
        try
        {
            var entity = BoxMapper.ToEntity(request);
            var created = await service.CreateAsync(entity);
            return TypedResults.Created($"/boxes/{created.Id}", BoxMapper.ToResponse(created));
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(new ErrorResponse(ex.Errors));
        }
    }
}

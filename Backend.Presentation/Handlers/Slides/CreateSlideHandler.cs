using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

/// <summary>Handles POST /slides — creates a new slide.</summary>
public static class CreateSlideHandler
{
    public static async Task<Results<Created<SlideResponse>, BadRequest<ErrorResponse>>> HandleAsync(
        [FromBody] CreateSlideRequest request,
        [FromServices] ISlideService service)
    {
        try
        {
            var entity = SlideMapper.ToEntity(request);
            var created = await service.CreateAsync(entity);
            return TypedResults.Created($"/slides/{created.Id}", SlideMapper.ToResponse(created));
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(new ErrorResponse(ex.Errors));
        }
    }
}

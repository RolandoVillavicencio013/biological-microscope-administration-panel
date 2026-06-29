using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Slides;

/// <summary>Handles PUT /slides/{id} — updates a slide's description and observations (RF-3).</summary>
public static class UpdateSlideHandler
{
    public static async Task<Results<Ok<SlideResponse>, BadRequest<ErrorResponse>, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        [FromBody] UpdateSlideRequest request,
        [FromServices] ISlideService service)
    {
        try
        {
            var existing = await service.FindByIdAsync(id);
            if (existing is null) return TypedResults.NotFound($"Slide {id} not found.");

            SlideMapper.ApplyUpdate(existing, request);
            var result = await service.UpdateAsync(existing);
            return TypedResults.Ok(SlideMapper.ToResponse(result));
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(new ErrorResponse(ex.Errors));
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Images;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Images;

/// <summary>Handles PUT /images/{id} — updates an image's display order.</summary>
public static class UpdateImageOrderHandler
{
    public static async Task<Results<Ok<ImageResponse>, BadRequest<ErrorResponse>, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        [FromBody] UpdateImageOrderRequest request,
        [FromServices] IImageService service)
    {
        try
        {
            var existing = await service.FindByIdAsync(id);
            if (existing is null) return TypedResults.NotFound($"Image {id} not found.");

            ImageMapper.ApplyOrderUpdate(existing, request.Order);
            var result = await service.UpdateAsync(existing);
            return TypedResults.Ok(ImageMapper.ToResponse(result));
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(new ErrorResponse(ex.Errors));
        }
    }
}

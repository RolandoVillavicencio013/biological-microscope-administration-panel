using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.EB.BioMicroscopeAdmin.Backend.Application.Services;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Images;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Handlers.Images;

/// <summary>Handles POST /slides/{id}/images — uploads an image file and associates it with a slide.</summary>
public static class UploadImageHandler
{
    public static async Task<Results<Created<ImageResponse>, BadRequest<ErrorResponse>, NotFound<string>>> HandleAsync(
        [FromRoute] int id,
        IFormFile file,
        [FromForm] int order,
        [FromServices] ISlideService slideService,
        [FromServices] IImageService imageService,
        [FromServices] IFileStorageService fileStorageService)
    {
        var errors = new List<ValidationError>();

        var slide = await slideService.FindByIdAsync(id);
        if (slide is null) return TypedResults.NotFound($"Slide {id} not found.");

        if (file is null || file.Length == 0)
            errors.Add(new("File", "A non-empty file is required."));

        if (!ImageOrder.TryCreate(order, out var imageOrder))
            errors.Add(new("Order", $"Order must be between {ImageOrder.Min} and {ImageOrder.Max}."));

        if (errors.Count > 0) return TypedResults.BadRequest(new ErrorResponse(errors));

        var fileUrl = await fileStorageService.SaveAsync(file!.OpenReadStream(), file.FileName);

        if (!ImageUrl.TryCreate(fileUrl, out var url))
            return TypedResults.BadRequest(new ErrorResponse([new("File", "Generated file URL is invalid.")]));

        if (!ImageFileName.TryCreate(file.FileName, out var fileName))
            return TypedResults.BadRequest(new ErrorResponse([new("File", $"File name must be at most {ImageFileName.MaxLength} characters.")]));

        var image = new Image(url!, fileName!, imageOrder!, id);
        var created = await imageService.CreateAsync(image);
        return TypedResults.Created($"/slides/{id}/images", ImageMapper.ToResponse(created));
    }
}

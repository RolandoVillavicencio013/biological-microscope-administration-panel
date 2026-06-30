using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Images;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Slides;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

/// <summary>Maps between <see cref="Slide"/> entities and their DTOs.</summary>
public static class SlideMapper
{
    /// <summary>Converts a <see cref="Slide"/> to a <see cref="SlideResponse"/> (without images).</summary>
    public static SlideResponse ToResponse(Slide slide) =>
        new(slide.Id, slide.Code.Value, slide.Name.Value, slide.BoxId,
            slide.Description?.Value, slide.Observations?.Value);

    /// <summary>Converts a <see cref="Slide"/> to a <see cref="SlideDetailResponse"/> (with images).</summary>
    public static SlideDetailResponse ToDetailResponse(Slide slide) =>
        new(slide.Id, slide.Code.Value, slide.Name.Value, slide.BoxId,
            slide.Description?.Value, slide.Observations?.Value,
            slide.Images.Select(ImageMapper.ToResponse).ToList());

    /// <summary>Converts a <see cref="CreateSlideRequest"/> to a new <see cref="Slide"/> entity.</summary>
    public static Slide ToEntity(CreateSlideRequest request)
    {
        var errors = new List<ValidationError>();

        if (!SlideCode.TryCreate(request.Code, out var code))
            errors.Add(new("Code", $"Must be non-empty and at most {SlideCode.MaxLength} characters."));

        if (!SlideName.TryCreate(request.Name, out var name))
            errors.Add(new("Name", $"Must be non-empty and at most {SlideName.MaxLength} characters."));

        Text? description = null;
        if (request.Description is not null && !Text.TryCreate(request.Description, out description))
            errors.Add(new("Description", "Must not be empty if provided."));

        Text? observations = null;
        if (request.Observations is not null && !Text.TryCreate(request.Observations, out observations))
            errors.Add(new("Observations", "Must not be empty if provided."));

        if (errors.Count > 0) throw new ValidationException(errors);

        return new Slide(code!, name!, request.BoxId, description, observations);
    }

    /// <summary>Applies <see cref="UpdateSlideRequest"/> fields onto an existing <see cref="Slide"/> entity.</summary>
    public static void ApplyUpdate(Slide slide, UpdateSlideRequest request)
    {
        var errors = new List<ValidationError>();

        Text? description = null;
        if (request.Description is not null && !Text.TryCreate(request.Description, out description))
            errors.Add(new("Description", "Must not be empty if provided."));

        Text? observations = null;
        if (request.Observations is not null && !Text.TryCreate(request.Observations, out observations))
            errors.Add(new("Observations", "Must not be empty if provided."));

        if (errors.Count > 0) throw new ValidationException(errors);

        slide.Description = description;
        slide.Observations = observations;
    }
}

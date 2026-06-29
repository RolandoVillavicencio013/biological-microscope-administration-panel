using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Boxes;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

/// <summary>Maps between <see cref="Box"/> entities and their DTOs.</summary>
public static class BoxMapper
{
    /// <summary>Converts a <see cref="Box"/> entity to a <see cref="BoxResponse"/>.</summary>
    public static BoxResponse ToResponse(Box box) =>
        new(box.Id, box.Number.Value, box.Name.Value, box.GroupId);

    /// <summary>Converts a <see cref="CreateBoxRequest"/> to a new <see cref="Box"/> entity.</summary>
    public static Box ToEntity(CreateBoxRequest request)
    {
        var errors = new List<ValidationError>();

        if (!BoxNumber.TryCreate(request.Number, out var number))
            errors.Add(new("Number", "Must be a positive integer."));

        if (!BoxName.TryCreate(request.Name, out var name))
            errors.Add(new("Name", $"Must be non-empty and at most {BoxName.MaxLength} characters."));

        if (errors.Count > 0) throw new ValidationException(errors);

        return new Box(number!, name!, request.GroupId);
    }

    /// <summary>Builds an updated <see cref="Box"/> entity preserving the existing id and group.</summary>
    public static Box ToUpdatedEntity(int id, int groupId, UpdateBoxRequest request)
    {
        var errors = new List<ValidationError>();

        if (!BoxNumber.TryCreate(request.Number, out var number))
            errors.Add(new("Number", "Must be a positive integer."));

        if (!BoxName.TryCreate(request.Name, out var name))
            errors.Add(new("Name", $"Must be non-empty and at most {BoxName.MaxLength} characters."));

        if (errors.Count > 0) throw new ValidationException(errors);

        return new Box(id, number!, name!, groupId);
    }
}

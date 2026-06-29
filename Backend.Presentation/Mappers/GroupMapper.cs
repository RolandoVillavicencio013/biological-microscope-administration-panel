using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;
using UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos.Groups;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Mappers;

/// <summary>Maps between <see cref="Group"/> entities and their DTOs.</summary>
public static class GroupMapper
{
    /// <summary>Converts a <see cref="Group"/> entity to a <see cref="GroupResponse"/>.</summary>
    public static GroupResponse ToResponse(Group group) =>
        new(group.Id, group.Name.Value);

    /// <summary>Converts a <see cref="CreateGroupRequest"/> to a new <see cref="Group"/> entity.</summary>
    public static Group ToEntity(CreateGroupRequest request)
    {
        var errors = new List<ValidationError>();

        if (!GroupName.TryCreate(request.Name, out var name))
            errors.Add(new("Name", $"Must be non-empty and at most {GroupName.MaxLength} characters."));

        if (errors.Count > 0) throw new ValidationException(errors);

        return new Group(name!);
    }

    /// <summary>Builds an updated <see cref="Group"/> entity preserving the existing id.</summary>
    public static Group ToUpdatedEntity(int id, CreateGroupRequest request)
    {
        var errors = new List<ValidationError>();

        if (!GroupName.TryCreate(request.Name, out var name))
            errors.Add(new("Name", $"Must be non-empty and at most {GroupName.MaxLength} characters."));

        if (errors.Count > 0) throw new ValidationException(errors);

        return new Group(id, name!);
    }
}

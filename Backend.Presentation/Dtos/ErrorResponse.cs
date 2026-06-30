using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Presentation.Dtos;

/// <summary>Response body returned when a request fails validation.</summary>
public record ErrorResponse(IReadOnlyList<ValidationError> Errors);

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

/// <summary>Details of a validation error: the affected field and a descriptive message.</summary>
public record ValidationError(string Field, string Message);

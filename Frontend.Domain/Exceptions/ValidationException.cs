namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.Exceptions;

/// <summary>Thrown when one or more domain fields fail validation.</summary>
public class ValidationException : DomainException
{
    /// <summary>List of validation errors, each with the affected field and failure reason.</summary>
    public IReadOnlyList<ValidationError> Errors { get; }

    public ValidationException() : base("Validation failed.")
    {
        Errors = [];
    }

    /// <summary>Creates the exception with a single error message and no specific field.</summary>
    public ValidationException(string message) : base(message)
    {
        Errors = [new(string.Empty, message)];
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
        Errors = [new(string.Empty, message)];
    }

    /// <summary>Creates the exception with multiple validation errors.</summary>
    public ValidationException(IEnumerable<ValidationError> errors) : base("Validation failed.")
    {
        Errors = [.. errors];
    }
}

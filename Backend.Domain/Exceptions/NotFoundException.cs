namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

/// <summary>Thrown when a requested resource does not exist in the system.</summary>
public class NotFoundException : DomainException
{
    public NotFoundException() { }
    public NotFoundException(string userFriendlyMessage) : base(userFriendlyMessage) { }
    public NotFoundException(string userFriendlyMessage, Exception innerException) : base(userFriendlyMessage, innerException) { }
}

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.Exceptions;

/// <summary>Base domain exception. All business exceptions inherit from this class.</summary>
public class DomainException : Exception
{
    public DomainException() { }
    public DomainException(string userFriendlyMessage) : base(userFriendlyMessage) { }
    public DomainException(string userFriendlyMessage, Exception innerException) : base(userFriendlyMessage, innerException) { }
}

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

/// <summary>Thrown when an entity creation would violate a uniqueness constraint.</summary>
public class DuplicatedEntityException : DomainException
{
    public DuplicatedEntityException() { }
    public DuplicatedEntityException(string userFriendlyMessage) : base(userFriendlyMessage) { }
    public DuplicatedEntityException(string userFriendlyMessage, Exception innerException) : base(userFriendlyMessage, innerException) { }
}

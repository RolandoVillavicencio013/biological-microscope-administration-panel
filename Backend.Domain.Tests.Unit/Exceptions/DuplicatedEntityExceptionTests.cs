using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.Exceptions;

public class DuplicatedEntityExceptionTests
{
    private const string Message = "A group named 'Bacteria' already exists.";

    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        // Act
        var exception = new DuplicatedEntityException(Message);

        // Assert
        Assert.Equal(Message, exception.Message);
    }

    [Fact]
    public void Instance_IsAssignableToDomainException()
    {
        // Act
        var exception = new DuplicatedEntityException(Message);

        // Assert
        Assert.IsAssignableFrom<DomainException>(exception);
    }
}

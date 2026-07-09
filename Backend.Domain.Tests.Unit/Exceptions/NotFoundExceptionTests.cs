using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.Exceptions;

public class NotFoundExceptionTests
{
    private const string Message = "Box 5 not found.";

    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        // Act
        var exception = new NotFoundException(Message);

        // Assert
        Assert.Equal(Message, exception.Message);
    }

    [Fact]
    public void Instance_IsAssignableToDomainException()
    {
        // Act
        var exception = new NotFoundException(Message);

        // Assert
        Assert.IsAssignableFrom<DomainException>(exception);
    }
}

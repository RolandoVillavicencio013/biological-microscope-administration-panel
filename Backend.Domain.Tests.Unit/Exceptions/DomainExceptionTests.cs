using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.Exceptions;

public class DomainExceptionTests
{
    private const string Message = "Something went wrong.";

    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        // Act
        var exception = new DomainException(Message);

        // Assert
        Assert.Equal(Message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsInnerException()
    {
        // Arrange
        var inner = new InvalidOperationException("cause");

        // Act
        var exception = new DomainException(Message, inner);

        // Assert
        Assert.Same(inner, exception.InnerException);
    }

    [Fact]
    public void Instance_IsAssignableToException()
    {
        // Act
        var exception = new DomainException(Message);

        // Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }
}

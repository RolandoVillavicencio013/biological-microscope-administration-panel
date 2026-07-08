using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.Exceptions;

public class ValidationExceptionTests
{
    private const string DefaultMessage = "Validation failed.";
    private const string SingleMessage = "Name must be non-empty.";

    [Fact]
    public void Constructor_Parameterless_SetsDefaultMessage()
    {
        // Act
        var exception = new ValidationException();

        // Assert
        Assert.Equal(DefaultMessage, exception.Message);
    }

    [Fact]
    public void Constructor_Parameterless_ErrorsIsEmpty()
    {
        // Act
        var exception = new ValidationException();

        // Assert
        Assert.Empty(exception.Errors);
    }

    [Fact]
    public void Constructor_WithSingleMessage_SetsMessage()
    {
        // Act
        var exception = new ValidationException(SingleMessage);

        // Assert
        Assert.Equal(SingleMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithSingleMessage_ErrorsContainsOneEntryWithEmptyField()
    {
        // Act
        var exception = new ValidationException(SingleMessage);

        // Assert
        var error = Assert.Single(exception.Errors);
        Assert.Equal(string.Empty, error.Field);
        Assert.Equal(SingleMessage, error.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsInnerException()
    {
        // Arrange
        var inner = new InvalidOperationException("cause");

        // Act
        var exception = new ValidationException(SingleMessage, inner);

        // Assert
        Assert.Same(inner, exception.InnerException);
    }

    [Fact]
    public void Constructor_WithErrorsCollection_MessageIsAlwaysValidationFailed()
    {
        // Arrange
        var errors = new[] { new ValidationError("Number", "Must be positive."), new ValidationError("Name", "Required.") };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.Equal(DefaultMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithErrorsCollection_PreservesAllErrors()
    {
        // Arrange
        var errors = new[] { new ValidationError("Number", "Must be positive."), new ValidationError("Name", "Required.") };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.Equal(errors, exception.Errors);
    }

    [Fact]
    public void Constructor_WithEmptyErrorsCollection_ErrorsIsEmpty()
    {
        // Act
        var exception = new ValidationException(Array.Empty<ValidationError>());

        // Assert
        Assert.Empty(exception.Errors);
    }

    [Fact]
    public void Instance_IsAssignableToDomainException()
    {
        // Act
        var exception = new ValidationException(SingleMessage);

        // Assert
        Assert.IsAssignableFrom<DomainException>(exception);
    }
}

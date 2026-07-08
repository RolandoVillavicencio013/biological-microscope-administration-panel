using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.ValueObjects;

public class BoxNumberTests
{
    private const int ValidNumber = 7;
    private const string ExpectedErrorMessage = "Box number must be a positive integer.";

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void TryCreate_ZeroOrNegativeValue_ReturnsFalse(int value)
    {
        // Act
        var succeeded = BoxNumber.TryCreate(value, out var result);

        // Assert
        Assert.False(succeeded);
        Assert.Null(result);
    }

    [Fact]
    public void TryCreate_PositiveValue_ReturnsTrue()
    {
        // Act
        var succeeded = BoxNumber.TryCreate(ValidNumber, out var result);

        // Assert
        Assert.True(succeeded);
        Assert.Equal(ValidNumber, result!.Value);
    }

    [Fact]
    public void Create_ValidValue_ReturnsInstance()
    {
        // Act
        var number = BoxNumber.Create(ValidNumber);

        // Assert
        Assert.Equal(ValidNumber, number.Value);
    }

    [Fact]
    public void Create_ZeroValue_ThrowsValidationExceptionWithExpectedMessage()
    {
        // Act
        var exception = Assert.Throws<ValidationException>(() => BoxNumber.Create(0));

        // Assert
        Assert.Equal(ExpectedErrorMessage, exception.Message);
    }

    [Fact]
    public void Equals_TwoInstancesWithSameValue_ReturnsTrue()
    {
        // Arrange
        var first = BoxNumber.Create(ValidNumber);
        var second = BoxNumber.Create(ValidNumber);

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void Equals_TwoInstancesWithDifferentValue_ReturnsFalse()
    {
        // Arrange
        var first = BoxNumber.Create(ValidNumber);
        var second = BoxNumber.Create(ValidNumber + 1);

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void GetHashCode_TwoInstancesWithSameValue_ReturnSameHashCode()
    {
        // Arrange
        var first = BoxNumber.Create(ValidNumber);
        var second = BoxNumber.Create(ValidNumber);

        // Act
        var firstHashCode = first.GetHashCode();
        var secondHashCode = second.GetHashCode();

        // Assert
        Assert.Equal(firstHashCode, secondHashCode);
    }
}

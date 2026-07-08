using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.ValueObjects;

public class BoxNameTests
{
    private const string ValidName = "Cyanobacteria";
    private const string ExpectedErrorMessage = "Box name must be non-empty and at most 150 characters.";

    [Fact]
    public void TryCreate_NullValue_ReturnsFalse()
    {
        // Act
        var succeeded = BoxName.TryCreate(null, out var result);

        // Assert
        Assert.False(succeeded);
        Assert.Null(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void TryCreate_EmptyOrWhitespaceValue_ReturnsFalse(string value)
    {
        // Act
        var succeeded = BoxName.TryCreate(value, out var result);

        // Assert
        Assert.False(succeeded);
        Assert.Null(result);
    }

    [Fact]
    public void TryCreate_ValueAtMaxLength_ReturnsTrue()
    {
        // Arrange
        var value = new string('a', BoxName.MaxLength);

        // Act
        var succeeded = BoxName.TryCreate(value, out var result);

        // Assert
        Assert.True(succeeded);
        Assert.Equal(value, result!.Value);
    }

    [Fact]
    public void TryCreate_ValueExceedingMaxLength_ReturnsFalse()
    {
        // Arrange
        var value = new string('a', BoxName.MaxLength + 1);

        // Act
        var succeeded = BoxName.TryCreate(value, out var result);

        // Assert
        Assert.False(succeeded);
        Assert.Null(result);
    }

    [Fact]
    public void TryCreate_ValueWithSurroundingWhitespace_TrimsValue()
    {
        // Act
        var succeeded = BoxName.TryCreate($"  {ValidName}  ", out var result);

        // Assert
        Assert.True(succeeded);
        Assert.Equal(ValidName, result!.Value);
    }

    [Fact]
    public void Create_ValidValue_ReturnsInstanceWithTrimmedValue()
    {
        // Act
        var name = BoxName.Create(ValidName);

        // Assert
        Assert.Equal(ValidName, name.Value);
    }

    [Fact]
    public void Create_NullValue_ThrowsValidationExceptionWithExpectedMessage()
    {
        // Act
        var exception = Assert.Throws<ValidationException>(() => BoxName.Create(null));

        // Assert
        Assert.Equal(ExpectedErrorMessage, exception.Message);
    }

    [Fact]
    public void Equals_TwoInstancesWithSameValue_ReturnsTrue()
    {
        // Arrange
        var first = BoxName.Create(ValidName);
        var second = BoxName.Create(ValidName);

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void Equals_TwoInstancesWithDifferentValue_ReturnsFalse()
    {
        // Arrange
        var first = BoxName.Create(ValidName);
        var second = BoxName.Create("Protozoa");

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void EqualityOperator_TwoInstancesWithSameValue_ReturnsFalse()
    {
        // Arrange
        var first = BoxName.Create(ValidName);
        var second = BoxName.Create(ValidName);

        // Act
        var areReferenceEqual = first == second;

        // Assert
        Assert.False(areReferenceEqual);
    }

    [Fact]
    public void GetHashCode_TwoInstancesWithSameValue_ReturnSameHashCode()
    {
        // Arrange
        var first = BoxName.Create(ValidName);
        var second = BoxName.Create(ValidName);

        // Act
        var firstHashCode = first.GetHashCode();
        var secondHashCode = second.GetHashCode();

        // Assert
        Assert.Equal(firstHashCode, secondHashCode);
    }
}

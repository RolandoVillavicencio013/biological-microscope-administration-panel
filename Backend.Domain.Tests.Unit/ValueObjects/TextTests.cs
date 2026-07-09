using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.ValueObjects;

public class TextTests
{
    private const string ValidText = "Sample collected from the lake shore.";
    private const string ExpectedErrorMessage = "Text value must be non-empty.";

    [Fact]
    public void TryCreate_NullValue_ReturnsFalse()
    {
        // Act
        var succeeded = Text.TryCreate(null, out var result);

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
        var succeeded = Text.TryCreate(value, out var result);

        // Assert
        Assert.False(succeeded);
        Assert.Null(result);
    }

    [Fact]
    public void TryCreate_ValueWithSurroundingWhitespace_TrimsValue()
    {
        // Act
        var succeeded = Text.TryCreate($"  {ValidText}  ", out var result);

        // Assert
        Assert.True(succeeded);
        Assert.Equal(ValidText, result!.Value);
    }

    [Fact]
    public void Create_ValidValue_ReturnsInstanceWithTrimmedValue()
    {
        // Act
        var text = Text.Create(ValidText);

        // Assert
        Assert.Equal(ValidText, text.Value);
    }

    [Fact]
    public void Create_NullValue_ThrowsValidationExceptionWithExpectedMessage()
    {
        // Act
        var exception = Assert.Throws<ValidationException>(() => Text.Create(null));

        // Assert
        Assert.Equal(ExpectedErrorMessage, exception.Message);
    }

    [Fact]
    public void Equals_TwoInstancesWithSameValue_ReturnsTrue()
    {
        // Arrange
        var first = Text.Create(ValidText);
        var second = Text.Create(ValidText);

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void Equals_TwoInstancesWithDifferentValue_ReturnsFalse()
    {
        // Arrange
        var first = Text.Create(ValidText);
        var second = Text.Create("A different observation.");

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void GetHashCode_TwoInstancesWithSameValue_ReturnSameHashCode()
    {
        // Arrange
        var first = Text.Create(ValidText);
        var second = Text.Create(ValidText);

        // Act
        var firstHashCode = first.GetHashCode();
        var secondHashCode = second.GetHashCode();

        // Assert
        Assert.Equal(firstHashCode, secondHashCode);
    }
}

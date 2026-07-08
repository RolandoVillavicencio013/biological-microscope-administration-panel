using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.ValueObjects;

public class ImageFileNameTests
{
    private const string ValidFileName = "sample.jpg";
    private const string ExpectedErrorMessage = "Image file name must be non-empty and at most 255 characters.";

    [Fact]
    public void TryCreate_NullValue_ReturnsFalse()
    {
        // Act
        var succeeded = ImageFileName.TryCreate(null, out var result);

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
        var succeeded = ImageFileName.TryCreate(value, out var result);

        // Assert
        Assert.False(succeeded);
        Assert.Null(result);
    }

    [Fact]
    public void TryCreate_ValueAtMaxLength_ReturnsTrue()
    {
        // Arrange
        var value = new string('a', ImageFileName.MaxLength);

        // Act
        var succeeded = ImageFileName.TryCreate(value, out var result);

        // Assert
        Assert.True(succeeded);
        Assert.Equal(value, result!.Value);
    }

    [Fact]
    public void TryCreate_ValueExceedingMaxLength_ReturnsFalse()
    {
        // Arrange
        var value = new string('a', ImageFileName.MaxLength + 1);

        // Act
        var succeeded = ImageFileName.TryCreate(value, out var result);

        // Assert
        Assert.False(succeeded);
        Assert.Null(result);
    }

    [Fact]
    public void TryCreate_ValueWithSurroundingWhitespace_TrimsValue()
    {
        // Act
        var succeeded = ImageFileName.TryCreate($"  {ValidFileName}  ", out var result);

        // Assert
        Assert.True(succeeded);
        Assert.Equal(ValidFileName, result!.Value);
    }

    [Fact]
    public void Create_ValidValue_ReturnsInstanceWithTrimmedValue()
    {
        // Act
        var fileName = ImageFileName.Create(ValidFileName);

        // Assert
        Assert.Equal(ValidFileName, fileName.Value);
    }

    [Fact]
    public void Create_NullValue_ThrowsValidationExceptionWithExpectedMessage()
    {
        // Act
        var exception = Assert.Throws<ValidationException>(() => ImageFileName.Create(null));

        // Assert
        Assert.Equal(ExpectedErrorMessage, exception.Message);
    }

    [Fact]
    public void Equals_TwoInstancesWithSameValue_ReturnsTrue()
    {
        // Arrange
        var first = ImageFileName.Create(ValidFileName);
        var second = ImageFileName.Create(ValidFileName);

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void Equals_TwoInstancesWithDifferentValue_ReturnsFalse()
    {
        // Arrange
        var first = ImageFileName.Create(ValidFileName);
        var second = ImageFileName.Create("other.png");

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void EqualityOperator_TwoInstancesWithSameValue_ReturnsFalse()
    {
        // Arrange
        var first = ImageFileName.Create(ValidFileName);
        var second = ImageFileName.Create(ValidFileName);

        // Act
        var areReferenceEqual = first == second;

        // Assert
        Assert.False(areReferenceEqual);
    }

    [Fact]
    public void GetHashCode_TwoInstancesWithSameValue_ReturnSameHashCode()
    {
        // Arrange
        var first = ImageFileName.Create(ValidFileName);
        var second = ImageFileName.Create(ValidFileName);

        // Act
        var firstHashCode = first.GetHashCode();
        var secondHashCode = second.GetHashCode();

        // Assert
        Assert.Equal(firstHashCode, secondHashCode);
    }
}

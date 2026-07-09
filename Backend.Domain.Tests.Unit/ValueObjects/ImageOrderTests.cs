using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.ValueObjects;

public class ImageOrderTests
{
    private const string ExpectedErrorMessage = "Image order must be between 1 and 6.";

    [Fact]
    public void TryCreate_ValueBelowMin_ReturnsFalse()
    {
        // Act
        var succeeded = ImageOrder.TryCreate(ImageOrder.Min - 1, out var result);

        // Assert
        Assert.False(succeeded);
        Assert.Null(result);
    }

    [Fact]
    public void TryCreate_ValueAboveMax_ReturnsFalse()
    {
        // Act
        var succeeded = ImageOrder.TryCreate(ImageOrder.Max + 1, out var result);

        // Assert
        Assert.False(succeeded);
        Assert.Null(result);
    }

    [Fact]
    public void TryCreate_ValueAtMin_ReturnsTrue()
    {
        // Act
        var succeeded = ImageOrder.TryCreate(ImageOrder.Min, out var result);

        // Assert
        Assert.True(succeeded);
        Assert.Equal(ImageOrder.Min, result!.Value);
    }

    [Fact]
    public void TryCreate_ValueAtMax_ReturnsTrue()
    {
        // Act
        var succeeded = ImageOrder.TryCreate(ImageOrder.Max, out var result);

        // Assert
        Assert.True(succeeded);
        Assert.Equal(ImageOrder.Max, result!.Value);
    }

    [Fact]
    public void Create_ValueOutsideRange_ThrowsValidationExceptionWithExpectedMessage()
    {
        // Act
        var exception = Assert.Throws<ValidationException>(() => ImageOrder.Create(ImageOrder.Max + 1));

        // Assert
        Assert.Equal(ExpectedErrorMessage, exception.Message);
    }

    [Fact]
    public void Equals_TwoInstancesWithSameValue_ReturnsTrue()
    {
        // Arrange
        var first = ImageOrder.Create(ImageOrder.Min);
        var second = ImageOrder.Create(ImageOrder.Min);

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void Equals_TwoInstancesWithDifferentValue_ReturnsFalse()
    {
        // Arrange
        var first = ImageOrder.Create(ImageOrder.Min);
        var second = ImageOrder.Create(ImageOrder.Max);

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void GetHashCode_TwoInstancesWithSameValue_ReturnSameHashCode()
    {
        // Arrange
        var first = ImageOrder.Create(ImageOrder.Min);
        var second = ImageOrder.Create(ImageOrder.Min);

        // Act
        var firstHashCode = first.GetHashCode();
        var secondHashCode = second.GetHashCode();

        // Assert
        Assert.Equal(firstHashCode, secondHashCode);
    }
}

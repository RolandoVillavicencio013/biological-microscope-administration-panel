using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.Tests.Unit.ValueObjects;

public class GroupNameTests
{
    private const string ValidName = "Bacteria";
    private const string ExpectedErrorMessage = "Group name must be non-empty and at most 100 characters.";

    [Fact]
    public void TryCreate_NullValue_ReturnsFalse()
    {
        // Act
        var succeeded = GroupName.TryCreate(null, out var result);

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
        var succeeded = GroupName.TryCreate(value, out var result);

        // Assert
        Assert.False(succeeded);
        Assert.Null(result);
    }

    [Fact]
    public void TryCreate_ValueAtMaxLength_ReturnsTrue()
    {
        // Arrange
        var value = new string('a', GroupName.MaxLength);

        // Act
        var succeeded = GroupName.TryCreate(value, out var result);

        // Assert
        Assert.True(succeeded);
        Assert.Equal(value, result!.Value);
    }

    [Fact]
    public void TryCreate_ValueExceedingMaxLength_ReturnsFalse()
    {
        // Arrange
        var value = new string('a', GroupName.MaxLength + 1);

        // Act
        var succeeded = GroupName.TryCreate(value, out var result);

        // Assert
        Assert.False(succeeded);
        Assert.Null(result);
    }

    [Fact]
    public void TryCreate_ValueWithSurroundingWhitespace_TrimsValue()
    {
        // Act
        var succeeded = GroupName.TryCreate($"  {ValidName}  ", out var result);

        // Assert
        Assert.True(succeeded);
        Assert.Equal(ValidName, result!.Value);
    }

    [Fact]
    public void Create_ValidValue_ReturnsInstanceWithTrimmedValue()
    {
        // Act
        var name = GroupName.Create(ValidName);

        // Assert
        Assert.Equal(ValidName, name.Value);
    }

    [Fact]
    public void Create_NullValue_ThrowsValidationExceptionWithExpectedMessage()
    {
        // Act
        var exception = Assert.Throws<ValidationException>(() => GroupName.Create(null));

        // Assert
        Assert.Equal(ExpectedErrorMessage, exception.Message);
    }

    [Fact]
    public void Equals_TwoInstancesWithSameValue_ReturnsTrue()
    {
        // Arrange
        var first = GroupName.Create(ValidName);
        var second = GroupName.Create(ValidName);

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void Equals_TwoInstancesWithDifferentValue_ReturnsFalse()
    {
        // Arrange
        var first = GroupName.Create(ValidName);
        var second = GroupName.Create("Fungi");

        // Act
        var areEqual = first.Equals(second);

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void EqualityOperator_TwoInstancesWithSameValue_ReturnsFalse()
    {
        // Arrange
        var first = GroupName.Create(ValidName);
        var second = GroupName.Create(ValidName);

        // Act
        var areReferenceEqual = first == second;

        // Assert
        Assert.False(areReferenceEqual);
    }

    [Fact]
    public void GetHashCode_TwoInstancesWithSameValue_ReturnSameHashCode()
    {
        // Arrange
        var first = GroupName.Create(ValidName);
        var second = GroupName.Create(ValidName);

        // Act
        var firstHashCode = first.GetHashCode();
        var secondHashCode = second.GetHashCode();

        // Assert
        Assert.Equal(firstHashCode, secondHashCode);
    }
}

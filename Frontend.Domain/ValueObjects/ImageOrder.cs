using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Frontend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.ValueObjects;

/// <summary>Display position of an image within its slide. Valid range: 1-6.</summary>
public sealed class ImageOrder : ValueObject
{
    public const int Min = 1;
    public const int Max = 6;

    /// <summary>The order value (1 to 6).</summary>
    public int Value { get; }

    private ImageOrder(int value) { Value = value; }

    public static bool TryCreate(int value, [NotNullWhen(true)] out ImageOrder? result)
    {
        result = null;
        if (value < Min || value > Max) return false;
        result = new(value);
        return true;
    }

    public static ImageOrder Create(int value)
    {
        if (!TryCreate(value, out var result) || result is null)
            throw new ValidationException($"Image order must be between {Min} and {Max}.");
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

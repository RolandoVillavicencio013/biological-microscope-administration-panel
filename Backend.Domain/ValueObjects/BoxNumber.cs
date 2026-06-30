using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

/// <summary>Physical number of a box. Must be a positive integer.</summary>
public sealed class BoxNumber : ValueObject
{
    /// <summary>The numeric value.</summary>
    public int Value { get; }

    private BoxNumber(int value) { Value = value; }

    /// <summary>Tries to create a <see cref="BoxNumber"/>. Returns false if the value is not positive.</summary>
    public static bool TryCreate(int value, [NotNullWhen(true)] out BoxNumber? result)
    {
        result = null;
        if (value <= 0) return false;
        result = new(value);
        return true;
    }

    /// <summary>Creates a <see cref="BoxNumber"/> or throws <see cref="ValidationException"/> if the value is invalid.</summary>
    public static BoxNumber Create(int value)
    {
        if (!TryCreate(value, out var result) || result is null)
            throw new ValidationException("Box number must be a positive integer.");
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

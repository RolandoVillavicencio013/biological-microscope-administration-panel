using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

/// <summary>Free-form non-empty text with no fixed length limit. Used for optional fields such as Description and Observations.</summary>
public sealed class Text : ValueObject
{
    /// <summary>The stored text value.</summary>
    public string Value { get; }

    private Text(string value) { Value = value; }

    /// <summary>Tries to create a <see cref="Text"/> from a string. Returns false if null or whitespace-only.</summary>
    public static bool TryCreate(string? value, [NotNullWhen(true)] out Text? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        result = new(value.Trim());
        return true;
    }

    /// <summary>Creates a <see cref="Text"/> or throws <see cref="ValidationException"/> if the value is invalid.</summary>
    public static Text Create(string? value)
    {
        if (!TryCreate(value, out var result) || result is null)
            throw new ValidationException("Text value must be non-empty.");
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Frontend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.ValueObjects;

/// <summary>Free-form non-empty text with no fixed length limit. Used for optional fields such as Description and Observations.</summary>
public sealed class Text : ValueObject
{
    /// <summary>The stored text value.</summary>
    public string Value { get; }

    private Text(string value) { Value = value; }

    public static bool TryCreate(string? value, [NotNullWhen(true)] out Text? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        result = new(value.Trim());
        return true;
    }

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

using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

/// <summary>Composite code of a slide (e.g. "1TB07"). Maximum 20 characters.</summary>
public sealed class SlideCode : ValueObject
{
    public const int MaxLength = 20;

    /// <summary>The code value.</summary>
    public string Value { get; }

    private SlideCode(string value) { Value = value; }

    /// <summary>Tries to create a <see cref="SlideCode"/>. Returns false if null, empty, or exceeds <see cref="MaxLength"/>.</summary>
    public static bool TryCreate(string? value, [NotNullWhen(true)] out SlideCode? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength) return false;
        result = new(trimmed);
        return true;
    }

    /// <summary>Creates a <see cref="SlideCode"/> or throws <see cref="ValidationException"/> if the value is invalid.</summary>
    public static SlideCode Create(string? value)
    {
        if (!TryCreate(value, out var result) || result is null)
            throw new ValidationException($"Slide code must be non-empty and at most {MaxLength} characters.");
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

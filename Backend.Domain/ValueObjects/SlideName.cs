using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

/// <summary>Name of a specific physical slide (e.g. "Gloecapsa"). Maximum 150 characters.</summary>
public sealed class SlideName : ValueObject
{
    public const int MaxLength = 150;

    /// <summary>The name value.</summary>
    public string Value { get; }

    private SlideName(string value) { Value = value; }

    /// <summary>Tries to create a <see cref="SlideName"/>. Returns false if null, empty, or exceeds <see cref="MaxLength"/>.</summary>
    public static bool TryCreate(string? value, [NotNullWhen(true)] out SlideName? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength) return false;
        result = new(trimmed);
        return true;
    }

    /// <summary>Creates a <see cref="SlideName"/> or throws <see cref="ValidationException"/> if the value is invalid.</summary>
    public static SlideName Create(string? value)
    {
        if (!TryCreate(value, out var result) || result is null)
            throw new ValidationException($"Slide name must be non-empty and at most {MaxLength} characters.");
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

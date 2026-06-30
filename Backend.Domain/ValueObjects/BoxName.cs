using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

/// <summary>Name of a physical box (e.g. "Cyanobacteria"). Maximum 150 characters.</summary>
public sealed class BoxName : ValueObject
{
    public const int MaxLength = 150;

    /// <summary>The name value.</summary>
    public string Value { get; }

    private BoxName(string value) { Value = value; }

    /// <summary>Tries to create a <see cref="BoxName"/>. Returns false if null, empty, or exceeds <see cref="MaxLength"/>.</summary>
    public static bool TryCreate(string? value, [NotNullWhen(true)] out BoxName? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength) return false;
        result = new(trimmed);
        return true;
    }

    /// <summary>Creates a <see cref="BoxName"/> or throws <see cref="ValidationException"/> if the value is invalid.</summary>
    public static BoxName Create(string? value)
    {
        if (!TryCreate(value, out var result) || result is null)
            throw new ValidationException($"Box name must be non-empty and at most {MaxLength} characters.");
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

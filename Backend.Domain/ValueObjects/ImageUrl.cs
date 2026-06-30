using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

/// <summary>File path or URL of an image associated with a slide. Maximum 500 characters.</summary>
public sealed class ImageUrl : ValueObject
{
    public const int MaxLength = 500;

    /// <summary>The URL or path value.</summary>
    public string Value { get; }

    private ImageUrl(string value) { Value = value; }

    /// <summary>Tries to create an <see cref="ImageUrl"/>. Returns false if null, empty, or exceeds <see cref="MaxLength"/>.</summary>
    public static bool TryCreate(string? value, [NotNullWhen(true)] out ImageUrl? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength) return false;
        result = new(trimmed);
        return true;
    }

    /// <summary>Creates an <see cref="ImageUrl"/> or throws <see cref="ValidationException"/> if the value is invalid.</summary>
    public static ImageUrl Create(string? value)
    {
        if (!TryCreate(value, out var result) || result is null)
            throw new ValidationException($"Image URL must be non-empty and at most {MaxLength} characters.");
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

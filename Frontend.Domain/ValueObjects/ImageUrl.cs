using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Frontend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.ValueObjects;

/// <summary>File path or URL of an image associated with a slide. Maximum 500 characters.</summary>
public sealed class ImageUrl : ValueObject
{
    public const int MaxLength = 500;

    /// <summary>The value.</summary>
    public string Value { get; }

    private ImageUrl(string value) { Value = value; }

    public static bool TryCreate(string? value, [NotNullWhen(true)] out ImageUrl? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength) return false;
        result = new(trimmed);
        return true;
    }

    public static ImageUrl Create(string? value)
    {
        if (!TryCreate(value, out var result) || result is null)
            throw new ValidationException("Image URL must be non-empty and at most 500 characters.");
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

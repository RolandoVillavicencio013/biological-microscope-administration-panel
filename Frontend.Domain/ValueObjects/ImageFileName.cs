using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Frontend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.ValueObjects;

/// <summary>Original file name of an image. Maximum 255 characters.</summary>
public sealed class ImageFileName : ValueObject
{
    public const int MaxLength = 255;

    /// <summary>The value.</summary>
    public string Value { get; }

    private ImageFileName(string value) { Value = value; }

    public static bool TryCreate(string? value, [NotNullWhen(true)] out ImageFileName? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength) return false;
        result = new(trimmed);
        return true;
    }

    public static ImageFileName Create(string? value)
    {
        if (!TryCreate(value, out var result) || result is null)
            throw new ValidationException("Image file name must be non-empty and at most 255 characters.");
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

/// <summary>Original file name of an image. Maximum 255 characters.</summary>
public sealed class ImageFileName : ValueObject
{
    public const int MaxLength = 255;

    /// <summary>The file name value.</summary>
    public string Value { get; }

    private ImageFileName(string value) { Value = value; }

    /// <summary>Tries to create an <see cref="ImageFileName"/>. Returns false if null, empty, or exceeds <see cref="MaxLength"/>.</summary>
    public static bool TryCreate(string? value, [NotNullWhen(true)] out ImageFileName? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength) return false;
        result = new(trimmed);
        return true;
    }

    /// <summary>Creates an <see cref="ImageFileName"/> or throws <see cref="ValidationException"/> if the value is invalid.</summary>
    public static ImageFileName Create(string? value)
    {
        if (!TryCreate(value, out var result) || result is null)
            throw new ValidationException($"Image file name must be non-empty and at most {MaxLength} characters.");
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

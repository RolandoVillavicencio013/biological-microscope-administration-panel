using System.Diagnostics.CodeAnalysis;
using UCR.EB.BioMicroscopeAdmin.Frontend.Domain.Exceptions;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.ValueObjects;

/// <summary>Name of a biological group (e.g. "Bacteria"). Maximum 100 characters.</summary>
public sealed class GroupName : ValueObject
{
    public const int MaxLength = 100;

    /// <summary>The value.</summary>
    public string Value { get; }

    private GroupName(string value) { Value = value; }

    public static bool TryCreate(string? value, [NotNullWhen(true)] out GroupName? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength) return false;
        result = new(trimmed);
        return true;
    }

    public static GroupName Create(string? value)
    {
        if (!TryCreate(value, out var result) || result is null)
            throw new ValidationException("Group name must be non-empty and at most 100 characters.");
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

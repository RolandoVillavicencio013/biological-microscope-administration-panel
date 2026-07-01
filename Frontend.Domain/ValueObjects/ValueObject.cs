namespace UCR.EB.BioMicroscopeAdmin.Frontend.Domain.ValueObjects;

/// <summary>Base class for all domain value objects. Implements equality by value.</summary>
public abstract class ValueObject
{
    /// <summary>Returns the components that define equality for this value object.</summary>
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;
        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode() =>
        GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);

    protected static bool EqualOperator(ValueObject? left, ValueObject? right) =>
        left is null ? right is null : left.Equals(right);

    protected static bool NotEqualOperator(ValueObject? left, ValueObject? right) =>
        !EqualOperator(left, right);
}

using Domain.Enums;

namespace Domain.ValueObjects;

public sealed class Currency(CurrencyCode code) : IEquatable<Currency>
{
    public CurrencyCode Code { get; } = code;

    public override bool Equals(object? obj)
        => Equals(obj as Currency);

    public bool Equals(Currency? other)
        => other is not null && Code == other.Code;

    public override int GetHashCode()
        => Code.GetHashCode();

    public override string ToString()
        => Code.ToString();
}
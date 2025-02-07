using PM.Domain.Common.Models;

namespace PM.Domain.CountryAggregate.ValueObjects;

/// <summary>
/// Country aggregate identifier
/// </summary>
public class CountryId : AggregateRootId<Guid>
{
    public sealed override Guid Value { get; protected set; }

    private CountryId(Guid value)
    {
        Value = value;
    }

    public static CountryId CreateUnique()
    {
        return new CountryId(Guid.NewGuid());
    }

    public static CountryId Create(Guid id)
    {
        return new(id);
    }

    public static implicit operator Guid(CountryId catalogId) => catalogId.Value;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

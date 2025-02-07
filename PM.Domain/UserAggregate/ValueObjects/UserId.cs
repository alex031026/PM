using PM.Domain.Common.Models;

namespace PM.Domain.UserAggregate.ValueObjects;

/// <summary>
/// User aggregate identifier
/// </summary>
public class UserId : AggregateRootId<Guid>
{
    public sealed override Guid Value { get; protected set; }

    private UserId(Guid value)
    {
        Value = value;
    }

    public static UserId CreateUnique()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId Create(Guid id)
    {
        return new(id);
    }

    public static implicit operator Guid(UserId catalogId) => catalogId.Value;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

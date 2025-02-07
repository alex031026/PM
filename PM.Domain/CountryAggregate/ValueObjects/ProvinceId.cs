using PM.Domain.Common.Models;

namespace PM.Domain.CountryAggregate.ValueObjects;

/// <summary>
/// Province entity identifier
/// </summary>
public class ProvinceId : ValueObject
{
    public Guid Value { get; protected set; }

    private ProvinceId(Guid value)
    {
        Value = value;
    }

    public static ProvinceId CreateUnique()
    {
        return new ProvinceId(Guid.NewGuid());
    }

    public static ProvinceId Create(Guid id)
    {
        return new(id);
    }

    public static implicit operator Guid(ProvinceId provinceId) => provinceId.Value;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
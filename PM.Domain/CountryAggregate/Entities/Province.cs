using PM.Domain.Common.Models;
using PM.Domain.CountryAggregate.ValueObjects;

namespace PM.Domain.CountryAggregate.Entities;

/// <summary>
/// Province Domain Entity
/// </summary>
public class Province : Entity<ProvinceId>
{
    /// <summary>
    /// Province name
    /// </summary>
    public string Name { get; set; }

    private Province(ProvinceId id, string name) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

        Name = name;
    }

    /// <summary>
    /// Create a new Province domain entity with unique identifier
    /// </summary>
    /// <param name="name">the province name</param>
    /// <returns>Province entity</returns>
    public static Province Create(string name)
    {
        return new(ProvinceId.CreateUnique(), name);
    }
}

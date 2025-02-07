using PM.Domain.Common.Models;
using PM.Domain.CountryAggregate.Entities;
using PM.Domain.CountryAggregate.ValueObjects;

namespace PM.Domain.CountryAggregate;

/// <summary>
/// Country Domain Aggregate
/// </summary>
public class Country : AggregateRoot<CountryId, Guid>
{
    /// <summary>
    /// Country name
    /// </summary>
    public string Name { get; }

    private readonly List<Province> _provinces = new();

    /// <summary>
    /// List of assigned provinces
    /// </summary>
    public IReadOnlyCollection<Province> Provinces => _provinces.AsReadOnly();

    private Country(CountryId id, string name) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

        Name = name;
    }

    /// <summary>
    /// Crate a new country aggregate with unique identifier
    /// </summary>
    /// <param name="name">the name of country </param>
    /// <returns>Country aggregate</returns>
    public static Country Create(string name)
    {
        return new Country(CountryId.CreateUnique(), name);
    }

    /// <summary>
    /// Adds a new province to the country
    /// </summary>
    /// <param name="name">the name of province</param>
    public void AddProvince(string name)
    {
        _provinces.Add(Province.Create(name));
    }
}


using PM.Application.Models.Country;
using PM.Domain.CountryAggregate.ValueObjects;

namespace PM.Application.Persistence.Repositories;

public interface ICountryReadOnlyRepository
{
    Task<Domain.CountryAggregate.Country?> GetById(CountryId id, CancellationToken cancellationToken = default);
    Task<List<CountryDto>> GetList(CancellationToken cancellationToken = default);
    Task<bool> ProvinceExists(ProvinceId provinceId, CancellationToken cancellationToken = default);
}


using ErrorOr;
using MediatR;
using PM.Application.Persistence.Repositories;
using PM.Domain.CountryAggregate.ValueObjects;

namespace PM.Application.Country.Queries;

/// <summary>
/// List of country provinces handler class
/// </summary>
public class ListCountryProvinceQueryHandler : IRequestHandler<ListCountryProvinceQuery, ErrorOr<Domain.CountryAggregate.Country?>>
{
    private readonly ICountryReadOnlyRepository _countryReadOnlyRepository;

    public ListCountryProvinceQueryHandler(ICountryReadOnlyRepository countryReadOnlyRepository)
    {
        _countryReadOnlyRepository = countryReadOnlyRepository;
    }

    /// <summary>
    /// Handles getting a country with list provinces
    /// </summary>
    public async Task<ErrorOr<Domain.CountryAggregate.Country?>> Handle(ListCountryProvinceQuery request, CancellationToken cancellationToken)
    {
        return await _countryReadOnlyRepository.GetById(CountryId.Create(request.CountryId), cancellationToken);
    }
}


using ErrorOr;
using MediatR;
using PM.Application.Models.Country;
using PM.Application.Persistence.Repositories;

namespace PM.Application.Country.Queries;

/// <summary>
/// List of countries handler class
/// </summary>
public class ListCountryQueryHandler : IRequestHandler<ListCountryQuery, ErrorOr<List<CountryDto>>>
{
    private readonly ICountryReadOnlyRepository _countryReadOnlyRepository;

    public ListCountryQueryHandler(ICountryReadOnlyRepository countryReadOnlyRepository)
    {
        _countryReadOnlyRepository = countryReadOnlyRepository;
    }

    /// <summary>
    /// Handles getting a list of countries
    /// </summary>
    public async Task<ErrorOr<List<CountryDto>>> Handle(ListCountryQuery request, CancellationToken cancellationToken)
    {
        return await _countryReadOnlyRepository.GetList(cancellationToken);
    }
}


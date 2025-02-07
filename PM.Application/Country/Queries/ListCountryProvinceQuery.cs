using ErrorOr;
using MediatR;

namespace PM.Application.Country.Queries;

public record ListCountryProvinceQuery(Guid CountryId) : IRequest<ErrorOr<Domain.CountryAggregate.Country?>>;


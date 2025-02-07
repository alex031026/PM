using ErrorOr;
using MediatR;
using PM.Application.Models.Country;

namespace PM.Application.Country.Queries;

public record ListCountryQuery : IRequest<ErrorOr<List<CountryDto>>>;


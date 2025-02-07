using FluentValidation;

namespace PM.Application.Country.Queries;

public class ListCountryProvinceQueryValidator : AbstractValidator<ListCountryProvinceQuery>
{
    public ListCountryProvinceQueryValidator()
    {
        RuleFor(m => m.CountryId)
            .NotNull()
            .NotEmpty();
    }
}

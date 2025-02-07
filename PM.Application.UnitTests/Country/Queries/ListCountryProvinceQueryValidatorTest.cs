using FluentValidation.TestHelper;
using PM.Application.Country.Queries;
using PM.Application.User.Queries;

namespace PM.Application.UnitTests.Country.Queries;

public class ListCountryProvinceQueryValidatorTest
{
    private readonly ListCountryProvinceQueryValidator _validator;

    public ListCountryProvinceQueryValidatorTest()
    {
        _validator = new ListCountryProvinceQueryValidator();
    }

    public static IEnumerable<object[]> ListCountryProvinceQueryValidData()
    {
        yield return new object[] {new ListCountryProvinceQuery(Guid.NewGuid())};
    }

    [Theory]
    [MemberData(nameof(ListCountryProvinceQueryValidData))]
    public void ValidateListCountryProvince_WhenIsValid_ShouldSuccess(ListCountryProvinceQuery query)
    {
        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.CountryId);
    }

    public static IEnumerable<object[]> ListCountryProvinceQueryInvalidData()
    {
        yield return new object[] {new ListCountryProvinceQuery(Guid.Empty)};
    }

    [Theory]
    [MemberData(nameof(ListCountryProvinceQueryInvalidData))]
    public void ValidateListCountryProvince_WhenIsNotValid_ShouldFail(ListCountryProvinceQuery query)
    {
        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CountryId);
    }
}


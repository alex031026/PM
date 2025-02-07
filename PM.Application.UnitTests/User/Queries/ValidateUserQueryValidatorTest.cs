using FluentValidation.TestHelper;
using PM.Application.User.Commands;
using PM.Application.User.Queries;

namespace PM.Application.UnitTests.User.Queries;

public class ListCountryProvinceQueryValidatorTest
{
    private readonly ValidateUserQueryValidator _validator;

    public ListCountryProvinceQueryValidatorTest()
    {
        _validator = new ValidateUserQueryValidator();
    }

    public static IEnumerable<object[]> ValidateUserQueryValidData()
    {
        yield return new object[] {new ValidateUserQuery("fake@email.com")};
        yield return new object[]
        {
            new ValidateUserQuery($"fake@email.com{new string('m', 306)}")
        };
    }

    [Theory]
    [MemberData(nameof(ValidateUserQueryValidData))]
    public void ValidateUserQuery_WhenIsValid_ShouldSuccess(ValidateUserQuery query)
    {
        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    public static IEnumerable<object[]> ValidateUserQueryInvalidData()
    {
        yield return new object[] {new ValidateUserQuery(null)};
        yield return new object[] {new ValidateUserQuery("")};
        yield return new object[] {new ValidateUserQuery("fake_email.com")};
        yield return new object[] {new ValidateUserQuery("fake@e@mail.com")};
        yield return new object[] {new ValidateUserQuery($"fake@email.com{new string('m', 307)}")};
    }

    [Theory]
    [MemberData(nameof(ValidateUserQueryInvalidData))]
    public void ValidateUserQuery_WhenIsNotValid_ShouldFail(ValidateUserQuery query)
    {
        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}


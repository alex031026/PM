using FluentValidation.TestHelper;
using PM.Application.User.Commands;

namespace PM.Application.UnitTests.User.Commands;

public class ValidateUserQueryValidatorTest
{
    private readonly CreateUserCommandValidator _validator;

    public ValidateUserQueryValidatorTest()
    {
        _validator = new CreateUserCommandValidator();
    }

    public static IEnumerable<object[]> CreateUserCommandValidData()
    {
        yield return new object[] {new CreateUserCommand("fake@email.com", "P1", Guid.NewGuid())};
        yield return new object[]
        {
            new CreateUserCommand($"fake@email.com{new string('m', 306)}", $"1{new string('c', 511)}", Guid.NewGuid())
        };
    }

    [Theory]
    [MemberData(nameof(CreateUserCommandValidData))]
    public void ValidateCreateUserCommand_WhenIsValid_ShouldSuccess(CreateUserCommand command)
    {
        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
        result.ShouldNotHaveValidationErrorFor(x => x.ProvinceId);
    }

    public static IEnumerable<object[]> CreateUserCommandInvalidData()
    {
        yield return new object[] {new CreateUserCommand(null, null, Guid.Empty)};
        yield return new object[] {new CreateUserCommand("", "123", Guid.Empty)};
        yield return new object[] {new CreateUserCommand(null, "", Guid.Empty)};
        yield return new object[] {new CreateUserCommand("fake_email.com", "P", Guid.Empty)};
        yield return new object[] {new CreateUserCommand("fake@e@mail.com", $"1{new string('c', 512)}", Guid.Empty)};
        yield return new object[] {new CreateUserCommand($"fake@email.com{new string('m', 307)}", "pp", Guid.Empty)};
    }

    [Theory]
    [MemberData(nameof(CreateUserCommandInvalidData))]
    public void ValidateCreateUserCommand_WhenIsNotValid_ShouldFail(CreateUserCommand command)
    {
        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldHaveValidationErrorFor(x => x.ProvinceId);
    }
}


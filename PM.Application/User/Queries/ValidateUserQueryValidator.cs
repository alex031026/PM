using FluentValidation;

namespace PM.Application.User.Queries;

public class ValidateUserQueryValidator : AbstractValidator<ValidateUserQuery>
{
    public ValidateUserQueryValidator()
    {
        RuleFor(m => m.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress()
            .MaximumLength(320);
    }
}


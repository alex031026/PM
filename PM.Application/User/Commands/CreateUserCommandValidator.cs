using FluentValidation;

namespace PM.Application.User.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(m => m.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress()
            .MaximumLength(320);

        RuleFor(m => m.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(512)
            .Matches("^(?=.*[A-Za-z])(?=.*\\d).+$");

        RuleFor(m => m.ProvinceId)
            .NotEmpty();
    }
}


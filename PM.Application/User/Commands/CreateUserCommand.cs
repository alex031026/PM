using ErrorOr;
using MediatR;

namespace PM.Application.User.Commands;

public record CreateUserCommand(
        string Email,
        string Password,
        Guid ProvinceId)
    : IRequest<ErrorOr<Domain.UserAggregate.User>>;


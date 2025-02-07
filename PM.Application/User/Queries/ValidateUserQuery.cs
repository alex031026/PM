using ErrorOr;
using MediatR;

namespace PM.Application.User.Queries;
public record ValidateUserQuery(
        string Email)
    : IRequest<ErrorOr<bool>>;

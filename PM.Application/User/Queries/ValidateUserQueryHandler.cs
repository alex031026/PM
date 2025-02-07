using ErrorOr;
using MediatR;
using PM.Application.Common.Errors;
using PM.Application.Common.Providers;
using PM.Application.Persistence.Repositories;
using PM.Application.User.Commands;

namespace PM.Application.User.Queries;

/// <summary>
/// Validate user query handler
/// </summary>
public class ValidateUserQueryHandler : IRequestHandler<ValidateUserQuery, ErrorOr<bool>>
{
    private readonly IUserRepository _userRepo;

    public ValidateUserQueryHandler(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    /// <summary>
    /// Handles user validation operations
    /// </summary>
    public async Task<ErrorOr<bool>> Handle(ValidateUserQuery request,
        CancellationToken cancellationToken)
    {
        // Check for email duplication
        if (await _userRepo.Exists(request.Email, cancellationToken))
            return Errors.User.DuplicateEmail;

        return true;
    }
}



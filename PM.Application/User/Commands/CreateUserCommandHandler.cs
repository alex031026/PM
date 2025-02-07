using ErrorOr;
using MediatR;
using PM.Application.Common.Errors;
using PM.Application.Common.Providers;
using PM.Application.Persistence.Repositories;
using PM.Domain.CountryAggregate.ValueObjects;
using PM.Domain.UserAggregate.ValueObjects;

namespace PM.Application.User.Commands;

/// <summary>
/// Create user command handler
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<Domain.UserAggregate.User>>
{
    private readonly IUserRepository _userRepo;
    private readonly ICountryReadOnlyRepository _countryRepo;
    private readonly IHashProvider _hashProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateUserCommandHandler(IUserRepository userRepo,
        ICountryReadOnlyRepository countryRepo,
        IHashProvider hashProvider,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepo = userRepo;
        _countryRepo = countryRepo;
        _hashProvider = hashProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// Handles user creation operations
    /// </summary>
    public async Task<ErrorOr<Domain.UserAggregate.User>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        // Check for email duplication
        if (await _userRepo.Exists(request.Email, cancellationToken))
            return Errors.User.DuplicateEmail;

        var provinceId = ProvinceId.Create(request.ProvinceId);
        // Check for province availability
        if (!await _countryRepo.ProvinceExists(provinceId, cancellationToken))
            return Errors.User.UnavailableProvince;

        // Get password hash
        var passwordHash = _hashProvider.GetHash(request.Password);

        // Create user domain entity
        var user = Domain.UserAggregate.User.Create(
            request.Email,
            passwordHash,
            provinceId,
            _dateTimeProvider.UtcNow);

        // Add user to repo
        user = _userRepo.Add(user);

        // Commit changes to repo
        await _userRepo.UnitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }
}

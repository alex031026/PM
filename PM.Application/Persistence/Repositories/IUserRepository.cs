namespace PM.Application.Persistence.Repositories;

public interface IUserRepository
{
    IUnitOfWork UnitOfWork { get; }

    Task<bool> Exists(string email, CancellationToken cancellationToken = default);

    Domain.UserAggregate.User Add(Domain.UserAggregate.User user);
}


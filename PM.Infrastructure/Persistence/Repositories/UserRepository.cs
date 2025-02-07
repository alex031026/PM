using Microsoft.EntityFrameworkCore;
using PM.Application.Persistence;
using PM.Application.Persistence.Repositories;
using PM.Domain.UserAggregate;

namespace PM.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PMDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public UserRepository(PMDbContext context)
    {
        _context = context;
    }

    public Task<bool> Exists(string email, CancellationToken cancellationToken = default)
    {
        return _context.Users.AsNoTracking()
            .AnyAsync(u => u.Email.Equals(email), cancellationToken);
    }

    public User Add(User user)
    {
        return _context.Users.Add(user).Entity;
    }
}

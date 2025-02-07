using PM.Domain.Common.Models;
using PM.Domain.CountryAggregate.ValueObjects;
using PM.Domain.UserAggregate.ValueObjects;

namespace PM.Domain.UserAggregate;

/// <summary>
/// User aggregate class
/// </summary>
public class User : AggregateRoot<UserId, Guid>

{
    public string Email { get; }
    public string PasswordHash { get; }
    public ProvinceId ProvinceId { get; }
    public DateTime CreatedDateUtc { get; }

    private User(UserId id, string email, string passwordHash, ProvinceId provinceId,
        DateTime createdDateUtc) : base(id)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentNullException(nameof(passwordHash));
        if (provinceId == null) throw new ArgumentNullException(nameof(provinceId));

        Email = email;
        PasswordHash = passwordHash;
        ProvinceId = provinceId;
        CreatedDateUtc = createdDateUtc;
    }

    /// <summary>
    /// Creates a new user with unique identifier
    /// </summary>
    /// <param name="email">user email</param>
    /// <param name="passwordHash">user password hash</param>
    /// <param name="provinceId">province assigned to the user</param>
    /// <param name="createdDateUtc">date and time in UTC when user created</param>
    /// <returns>User aggregate</returns>
    public static User Create(string email, string passwordHash, ProvinceId provinceId, DateTime createdDateUtc)
    {
        return new User(UserId.CreateUnique(),
            email,
            passwordHash,
            provinceId,
            createdDateUtc
        );
    }
}

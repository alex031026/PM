using PM.Domain.CountryAggregate.ValueObjects;
using PM.Domain.UserAggregate;
using System;

namespace PM.Domain.UnitTests;

public class UserAggregateTests
{
    [Fact]
    public void UserCreateUser_WhenIsValid_ShouldSuccess()
    {
        //Arrange
        const string email = "fake@email.com";
        const string passwordHash = "password_hash";
        var provinceId = ProvinceId.CreateUnique();

        //Act
        var user = User.Create(email, passwordHash, provinceId, DateTime.UtcNow);

        //Assert
        Assert.NotNull(user);
    }


    public static IEnumerable<object[]> CreateUserFailData()
    {
        yield return new object[] {"", "password_hash", ProvinceId.CreateUnique()};
        yield return new object[] {"fake@email.com", "", ProvinceId.CreateUnique()};
        yield return new object[] {"fake@email.com", "password_hash", null};
    }

    [Theory]
    [MemberData(nameof(CreateUserFailData))]
    public void UserCreateUser_WhenIsNotValid_ShouldFail(string email, string passwordHash, ProvinceId provinceId)
    {
        //Act - Assert
        Assert.Throws<ArgumentNullException>(() => User.Create(email, passwordHash, provinceId, DateTime.UtcNow));
    }
}

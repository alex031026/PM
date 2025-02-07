using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ExceptionExtensions;
using PM.Application.Common.Errors;
using PM.Application.Common.Providers;
using PM.Application.Persistence.Repositories;
using PM.Application.User.Commands;
using PM.Domain.CountryAggregate.ValueObjects;

namespace PM.Application.UnitTests.User.Commands;

public class ValidateUserQueryHandlerTest
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly ICountryReadOnlyRepository _countryRepositoryMock;
    private readonly CreateUserCommandHandler _handler;

    public ValidateUserQueryHandlerTest()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _countryRepositoryMock = Substitute.For<ICountryReadOnlyRepository>();

        var hashProviderMock = Substitute.For<IHashProvider>();
        var dateTimeProviderMock = Substitute.For<IDateTimeProvider>();

        _handler = new CreateUserCommandHandler(_userRepositoryMock, _countryRepositoryMock, hashProviderMock, dateTimeProviderMock);

        // Default behavior
        hashProviderMock.GetHash(Arg.Any<string>()).Returns("HASH");
        dateTimeProviderMock.UtcNow.Returns(DateTime.MaxValue);

        _userRepositoryMock.Add(Arg.Any<Domain.UserAggregate.User>())
            .Returns(x => x.Arg<Domain.UserAggregate.User>());

        _userRepositoryMock.UnitOfWork.SaveChangesAsync().Returns(Task.FromResult(1));

        _countryRepositoryMock.ProvinceExists(Arg.Any<ProvinceId>(), CancellationToken.None)
            .Returns(true);
    }

    [Fact]
    public async Task HandleCreateUserCommand_WhenUserIsValid_ShouldCreateAndReturnUser()
    {
        // Arrange
        const string email = "fake@email.com";
        const string password = "Pas123";
        var provinceId = Guid.NewGuid();

        var command = new CreateUserCommand(email, password, provinceId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal(email, result.Value.Email);
        Assert.Equal("HASH", result.Value.PasswordHash);
        Assert.Equal(DateTime.MaxValue, result.Value.CreatedDateUtc);
        Assert.Equal(provinceId, result.Value.ProvinceId.Value);
    }

    [Fact]
    public async Task HandleCreateUserCommand_WhenSameUserExists_ShouldReturnError()
    {
        // Arrange
        const string email = "fake@email.com";
        const string password = "Pas123";
        var provinceId = Guid.NewGuid();

        var command = new CreateUserCommand(email, password, provinceId);

        _userRepositoryMock.Exists(Arg.Any<string>(), CancellationToken.None)
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
    }

    [Fact]
    public async Task HandleCreateUserCommand_WhenNotAvailProvince_ShouldReturnError()
    {
        // Arrange
        const string email = "fake@email.com";
        const string password = "Pas123";
        var provinceId = Guid.NewGuid();

        var command = new CreateUserCommand(email, password, provinceId);

        _countryRepositoryMock.ProvinceExists(Arg.Any<ProvinceId>(), CancellationToken.None)
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
    }


    [Fact]
    public async Task HandleCreateUserCommand_WhenUserExistsError_ShouldThrowException()
    {
        // Arrange
        const string email = "fake@email.com";
        const string password = "Pas123";
        var provinceId = Guid.NewGuid();

        var command = new CreateUserCommand(email, password, provinceId);

        _userRepositoryMock.Exists(Arg.Any<string>(), CancellationToken.None)
            .Throws(new Exception("test exception"));

        // Act - Assert
        var ex = await Assert.ThrowsAsync<Exception>(async () =>
        {
            _ = await _handler.Handle(command, CancellationToken.None);
        });

        Assert.Equal("test exception", ex.Message);
    }


    [Fact]
    public async Task HandleCreateUserCommand_WhenCreateUserError_ShouldThrowException()
    {
        // Arrange
        const string email = "fake@email.com";
        const string password = "Pas123";
        var provinceId = Guid.NewGuid();

        var command = new CreateUserCommand(email, password, provinceId);

        _userRepositoryMock.Exists(Arg.Any<string>(), CancellationToken.None)
            .Returns(false);

        _userRepositoryMock.Add(Arg.Any<Domain.UserAggregate.User>())
            .Throws(new Exception("test exception"));

        // Act - Assert
        var ex = await Assert.ThrowsAsync<Exception>(async () =>
        {
            _ = await _handler.Handle(command, CancellationToken.None);
        });

        Assert.Equal("test exception", ex.Message);
    }

    [Fact]
    public async Task HandleCreateUserCommand_WhenSaveChangesError_ShouldThrowException()
    {
        // Arrange
        const string email = "fake@email.com";
        const string password = "Pas123";
        var provinceId = Guid.NewGuid();

        var command = new CreateUserCommand(email, password, provinceId);

        _userRepositoryMock.Exists(Arg.Any<string>(), CancellationToken.None)
            .Returns(false);

        _userRepositoryMock.UnitOfWork.SaveChangesAsync()
            .Throws(new Exception("test exception"));

        // Act - Assert
        var ex = await Assert.ThrowsAsync<Exception>(async () =>
        {
            _ = await _handler.Handle(command, CancellationToken.None);
        });

        Assert.Equal("test exception", ex.Message);
    }
}

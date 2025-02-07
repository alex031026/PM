using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PM.Application.Persistence.Repositories;
using PM.Application.User.Queries;

namespace PM.Application.UnitTests.User.Queries;

public class ListCountryQueryHandlerTest
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly ValidateUserQueryHandler _handler;

    public ListCountryQueryHandlerTest()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _handler = new ValidateUserQueryHandler(_userRepositoryMock);
    }

    [Fact]
    public async Task HandleValidateUserQuery_WhenUserIsValid_ShouldReturnTrue()
    {
        // Arrange
        const string email = "fake@email.com";
        var query = new ValidateUserQuery(email);

        _userRepositoryMock.Exists(Arg.Any<string>(), CancellationToken.None)
            .Returns(false);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task HandleValidateUserQuery_WhenEmailExists_ShouldReturnFalse()
    {
        // Arrange
        const string email = "fake@email.com";

        var query = new ValidateUserQuery(email);

        _userRepositoryMock.Exists(Arg.Any<string>(), CancellationToken.None)
            .Returns(true);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
    }

    [Fact]
    public async Task HandleValidateUserQuery_WhenError_ShouldThrowException()
    {
        // Arrange
        const string email = "fake@email.com";

        var query = new ValidateUserQuery(email);

        _userRepositoryMock.Exists(Arg.Any<string>(), CancellationToken.None)
            .Throws(new Exception("test exception"));

        // Act - Assert
        var ex = await Assert.ThrowsAsync<Exception>(async () =>
        {
            _ = await _handler.Handle(query, CancellationToken.None);
        });

        Assert.Equal("test exception", ex.Message);
    }
}

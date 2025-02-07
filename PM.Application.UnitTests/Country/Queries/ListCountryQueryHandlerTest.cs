using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PM.Application.Country.Queries;
using PM.Application.Models.Country;
using PM.Application.Persistence.Repositories;
using PM.Application.User.Queries;

namespace PM.Application.UnitTests.Country.Queries;

public class ListCountryQueryHandlerTest
{
    private readonly ICountryReadOnlyRepository _countryRepositoryMock;
    private readonly ListCountryQueryHandler _handler;

    public ListCountryQueryHandlerTest()
    {
        _countryRepositoryMock = Substitute.For<ICountryReadOnlyRepository>();
        _handler = new ListCountryQueryHandler(_countryRepositoryMock);
    }

    [Fact]
    public async Task HandleListCountryQuery_WhenNoErrors_ShouldReturnCountries()
    {
        // Arrange
        var query = new ListCountryQuery();

        _countryRepositoryMock.GetList()
            .Returns(new List<CountryDto>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task HandleListCountryQuery_WhenError_ShouldThrowException()
    {
        // Arrange
        var query = new ListCountryQuery();

        _countryRepositoryMock.GetList()
            .Throws(new Exception("test exception"));

        // Act - Assert
        var ex = await Assert.ThrowsAsync<Exception>(async () =>
        {
            _ = await _handler.Handle(query, CancellationToken.None);
        });

        Assert.Equal("test exception", ex.Message);
    }
}

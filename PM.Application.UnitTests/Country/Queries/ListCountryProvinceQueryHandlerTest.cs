using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PM.Application.Country.Queries;
using PM.Application.Models.Country;
using PM.Application.Persistence.Repositories;
using PM.Domain.CountryAggregate.ValueObjects;

namespace PM.Application.UnitTests.Country.Queries;

public class ListCountryProvinceQueryHandlerTest
{
    private readonly ICountryReadOnlyRepository _countryRepositoryMock;
    private readonly ListCountryProvinceQueryHandler _handler;

    public ListCountryProvinceQueryHandlerTest()
    {
        _countryRepositoryMock = Substitute.For<ICountryReadOnlyRepository>();
        _handler = new ListCountryProvinceQueryHandler(_countryRepositoryMock);
    }

    [Fact]
    public async Task HandleListCountryProvinceQuery_WhenNoErrors_ShouldReturnCountries()
    {
        // Arrange
        Guid countryId = Guid.NewGuid();
        var query = new ListCountryProvinceQuery(countryId);

        _countryRepositoryMock.GetById(Arg.Any<CountryId>())
            .Returns(Domain.CountryAggregate.Country.Create("CountryName"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value.Provinces);
    }

    [Fact]
    public async Task HandleListCountryProvinceQuery_WhenError_ShouldThrowException()
    {
        // Arrange
        Guid countryId = Guid.NewGuid();
        var query = new ListCountryProvinceQuery(countryId);

        _countryRepositoryMock.GetById(Arg.Any<CountryId>())
            .Throws(new Exception("test exception"));

        // Act - Assert
        var ex = await Assert.ThrowsAsync<Exception>(async () =>
        {
            _ = await _handler.Handle(query, CancellationToken.None);
        });

        Assert.Equal("test exception", ex.Message);
    }
}

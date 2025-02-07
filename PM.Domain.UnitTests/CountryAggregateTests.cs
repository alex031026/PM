using PM.Domain.CountryAggregate;
using PM.Domain.CountryAggregate.Entities;
using PM.Domain.CountryAggregate.ValueObjects;

namespace PM.Domain.UnitTests;

public class CountryAggregateTests
{
    [Fact]
    public void CountryCreateCountry_WhenIsValid_ShouldSuccess()
    {
        //Arrange
        const string name = "USA";

        //Act
        var country = Country.Create(name);

        //Assert
        Assert.NotNull(country);
    }

    [Fact]
    public void CountryCreateCountry_WhenIsNotValid_ShouldFail()
    {
        //Arrange
        const string name = "";

        //Act - Assert
        Assert.Throws<ArgumentNullException>(() => Country.Create(name));
    }

    [Fact]
    public void ProvinceCreateProvince_WhenIsValid_ShouldSuccess()
    {
        //Arrange
        const string name = "Texas";

        //Act
        var province = Province.Create(name);

        //Assert
        Assert.NotNull(province);
    }

    [Fact]
    public void ProvinceCreateProvince_WhenIsNotValid_ShouldFail()
    {
        //Arrange
        const string name = "";

        //Act - Assert
        Assert.Throws<ArgumentNullException>(() => Province.Create(name));
    }
}

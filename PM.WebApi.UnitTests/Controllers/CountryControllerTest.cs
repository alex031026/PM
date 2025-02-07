using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PM.Application.Country.Queries;
using PM.Application.Models.Country;
using PM.Contracts.Country;
using PM.Domain.CountryAggregate;
using PM.WebApi.Common.Mapping;
using PM.WebApi.Controllers;

namespace PM.WebApi.UnitTests.Controllers;

public class CountryControllerTest
{
    private readonly IMediator _mediatorMock;

    private readonly CountryController _controller;

    public CountryControllerTest()
    {
        _mediatorMock = Substitute.For<IMediator>();
        var loggerMock = Substitute.For<ILogger<CountryController>>();

        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
        config.Apply(new CountryMappingConfig());
        var mapper = new Mapper(config);

        _controller = new CountryController(loggerMock, _mediatorMock, mapper);
    }

    [Fact]
    public async Task List_WhenNoErrors_ShouldReturnOkWithCountries()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        const string name = "CountryName";

        _mediatorMock.Send(Arg.Any<ListCountryQuery>())
            .Returns(new List<CountryDto> {new() {Id = id, Name = name}});

        // Act
        var result = await _controller.List();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<List<CountryViewItem>>(okResult.Value);
        Assert.True(response.All(c => c.Id == id && c.Name.Equals(name)));
    }


    [Fact]
    public async Task List_OnException_ShouldReturnProblem()
    {
        // Arrange
        _mediatorMock.Send(Arg.Any<ListCountryQuery>())
            .Throws(new Exception("test exception"));

        // Act
        var result = await _controller.List();

        // Assert
        var errResult = Assert.IsType<ObjectResult>(result);
        var probResult = Assert.IsType<ProblemDetails>(errResult.Value);
        Assert.Equal("test exception", probResult.Detail);
    }


    [Fact]
    public async Task ListProvinces_WhenNoErrors_ShouldReturnOkWithProvinces()
    {
        // Arrange
        Guid countryId = Guid.NewGuid();
        const string name = "CountryName";

        _mediatorMock.Send(Arg.Any<ListCountryProvinceQuery>())
            .Returns(Country.Create(name));

        // Act
        var result = await _controller.ListProvinces(countryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<List<CountryProvinceViewItem>>(okResult.Value);
        Assert.NotNull(response);
    }

    [Fact]
    public async Task ListProvinces_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        Guid countryId = Guid.NewGuid();
        const string name = "CountryName";

        _mediatorMock.Send(Arg.Any<ListCountryProvinceQuery>())
            .Returns((Country?) null);

        // Act
        var result = await _controller.ListProvinces(countryId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ListProvinces_OnException_ShouldReturnProblem()
    {
        // Arrange
        Guid countryId = Guid.NewGuid();
        const string name = "CountryName";

        _mediatorMock.Send(Arg.Any<ListCountryProvinceQuery>())
            .Throws(new Exception("test exception"));

        // Act
        var result = await _controller.ListProvinces(countryId);

        // Assert
        var errResult = Assert.IsType<ObjectResult>(result);
        var probResult = Assert.IsType<ProblemDetails>(errResult.Value);
        Assert.Equal("test exception", probResult.Detail);
    }
}
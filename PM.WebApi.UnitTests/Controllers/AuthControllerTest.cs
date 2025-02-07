using ErrorOr;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PM.Application.Common.Errors;
using PM.Application.User.Commands;
using PM.Application.User.Queries;
using PM.Contracts.Auth;
using PM.Domain.CountryAggregate.ValueObjects;
using PM.Domain.UserAggregate;
using PM.WebApi.Common.Mapping;
using PM.WebApi.Controllers;

namespace PM.WebApi.UnitTests.Controllers;

public class AuthControllerTest
{
    private readonly IMediator _mediatorMock;

    private readonly AuthController _controller;

    public AuthControllerTest()
    {
        _mediatorMock = Substitute.For<IMediator>();
        var loggerMock = Substitute.For<ILogger<AuthController>>();

        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
        config.Apply(new AuthMappingConfig());
        var mapper = new Mapper(config);

        _controller = new AuthController(loggerMock, _mediatorMock, mapper);
    }

    [Fact]
    public async Task Register_WhenRequestIsValid_ShouldReturnOk()
    {
        // Arrange
        const string email = "fake@email.com";
        const string password = "Pass123";
        Guid provinceId = Guid.NewGuid();

        var request = new RegisterRequest(email, password, provinceId);

        _mediatorMock.Send(Arg.Any<CreateUserCommand>())
            .Returns(x =>
            {
                var command = x.Arg<CreateUserCommand>();
                return User.Create(command.Email, "hash", ProvinceId.Create(command.ProvinceId), DateTime.MaxValue);
            });

        // Act
        var result = await _controller.Register(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<RegisterResponse>(okResult.Value);
        Assert.Equal(email, response.Email);
        Assert.Equal(provinceId, response.ProvinceId);
    }

    [Fact]
    public async Task Register_WhenEmailDuplication_ShouldReturnProblem()
    {
        // Arrange
        const string email = "fake@email.com";
        const string password = "Pass123";
        Guid provinceId = Guid.NewGuid();

        var request = new RegisterRequest(email, password, provinceId);

        _mediatorMock.Send(Arg.Any<CreateUserCommand>())
            .Returns(Errors.User.DuplicateEmail);

        // Act
        var result = await _controller.Register(request);

        // Assert
        var errResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status409Conflict, errResult.StatusCode);
        var probResult = Assert.IsType<ProblemDetails>(errResult.Value);
        Assert.Equal("User.DuplicateEmail", probResult.Title);
        Assert.Equal("User with same email already exists.", probResult.Detail);
    }

    [Fact]
    public async Task Register_WhenCommandIsNotValid_ShouldReturnProblem()
    {
        // Arrange
        const string email = "";
        const string password = "Pass123";
        Guid provinceId = Guid.NewGuid();

        var request = new RegisterRequest(email, password, provinceId);

        _mediatorMock.Send(Arg.Any<CreateUserCommand>())
            .Returns(new List<Error> {Error.Validation("Email", "Email is required")});

        // Act
        var result = await _controller.Register(request);

        // Assert
        var errResult = Assert.IsType<ObjectResult>(result);
        var probResult = Assert.IsType<ValidationProblemDetails>(errResult.Value);
        Assert.Equal("Email is required", probResult.Errors["Email"][0]);
    }

    [Fact]
    public async Task Register_OnException_ShouldReturnProblem()
    {
        // Arrange
        const string email = "";
        const string password = "Pass123";
        Guid provinceId = Guid.NewGuid();

        var request = new RegisterRequest(email, password, provinceId);

        _mediatorMock.Send(Arg.Any<CreateUserCommand>())
            .Throws(new Exception("test exception"));

        // Act
        var result = await _controller.Register(request);

        // Assert
        var errResult = Assert.IsType<ObjectResult>(result);
        var probResult = Assert.IsType<ProblemDetails>(errResult.Value);
        Assert.Equal("test exception", probResult.Detail);
    }


    // Validate tests

    [Fact]
    public async Task Validate_WhenNoErrors_ShouldReturnOkRequest()
    {
        // Arrange
        const string email = "";

        var request = new ValidateRequest(email);

        _mediatorMock.Send(Arg.Any<ValidateUserQuery>())
            .Returns(true);

        // Act
        var result = await _controller.Validate(request);

        // Assert
        Assert.IsType<OkResult>(result);
    }


    [Fact]
    public async Task Validate_WhenError_ShouldReturnProblem()
    {
        // Arrange
        const string email = "";

        var request = new ValidateRequest(email);

        _mediatorMock.Send(Arg.Any<ValidateUserQuery>())
            .Returns(Errors.User.DuplicateEmail);

        // Act
        var result = await _controller.Validate(request);

        // Assert
        var errResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status409Conflict, errResult.StatusCode);
        var probResult = Assert.IsType<ProblemDetails>(errResult.Value);
        Assert.Equal("User.DuplicateEmail", probResult.Title);
        Assert.Equal("User with same email already exists.", probResult.Detail);
    }


    [Fact]
    public async Task Validate_OnException_ShouldReturnProblem()
    {
        // Arrange
        const string email = "";

        var request = new ValidateRequest(email);

        _mediatorMock.Send(Arg.Any<ValidateUserQuery>())
            .Throws(new Exception("test exception"));

        // Act
        var result = await _controller.Validate(request);

        // Assert
        var errResult = Assert.IsType<ObjectResult>(result);
        var probResult = Assert.IsType<ProblemDetails>(errResult.Value);
        Assert.Equal("test exception", probResult.Detail);
    }
}
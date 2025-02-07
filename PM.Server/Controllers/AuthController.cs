using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.Application.User.Commands;
using PM.Application.User.Queries;
using PM.Contracts.Auth;
using System.Net;

namespace PM.WebApi.Controllers;


/// <summary>
/// Handles operations with auth user.
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly ILogger<AuthController> _logger;
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthController(ILogger<AuthController> logger, ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new user 
    /// </summary>
    /// <param name="request">The user details for register</param>
    /// <returns>Ok:created user; Conflict; BadRequest</returns>
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    
    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            // Prepare command model
            var command = _mapper.Map<CreateUserCommand>(request);

            // Send command to mediator
            var result = await _mediator.Send(command);

            // Process result
            return result.Match(value => Ok(_mapper.Map<RegisterResponse>(value)), Problem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during auth register...");
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Validates user details
    /// </summary>
    /// <param name="request">The user details to validate</param>
    /// <returns>Ok;Problem,BadRequest</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [Route("validate")]
    [HttpPost]
    public async Task<IActionResult> Validate(ValidateRequest request)
    {
        try
        {
            // Prepare query model
            var query = _mapper.Map<ValidateUserQuery>(request);

            // Send query to mediator
            var result = await _mediator.Send(query);

            // Process result
            return result.Match(_ => Ok(), Problem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during auth validation...");
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}

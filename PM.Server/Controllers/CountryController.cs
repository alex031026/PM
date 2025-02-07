using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.Application.Country.Queries;
using PM.Contracts.Auth;
using PM.Contracts.Country;

namespace PM.WebApi.Controllers;

/// <summary>
/// Handles operations with country/province.
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[AllowAnonymous]
public class CountryController : ApiController
{
    private readonly ILogger<CountryController> _logger;
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public CountryController(ILogger<CountryController> logger, ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a list of countries
    /// </summary>
    /// <returns>Ok:list of countries;Problem</returns>
    [ProducesResponseType(typeof(List<CountryViewItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Route("list")]
    [HttpGet]
    public async Task<IActionResult> List()
    {
        try
        {
            // Send query to mediator
            var result = await _mediator.Send(new ListCountryQuery());

            // Process result
            return result.Match(list => Ok(_mapper.Map<List<CountryViewItem>>(list)), Problem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during getting a list of countries...");
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Gets a list of provinces
    /// </summary>
    /// <param name="countryId">The country identifier</param>
    /// <returns>OK:list of provinces;Conflict;Problem;NotFound</returns>
    [ProducesResponseType(typeof(List<CountryProvinceViewItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [Route("province/list")]
    [HttpGet]
    public async Task<IActionResult> ListProvinces(Guid countryId)
    {
        try
        {
            // Send query to mediator
            var result = await _mediator.Send(new ListCountryProvinceQuery(countryId));

            // Process result
            if (result.IsError)
                return Problem(result.Errors);

            if (result.Value is null)
                return NotFound("The country not found");

            return Ok(_mapper.Map<List<CountryProvinceViewItem>>(result.Value.Provinces));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during getting a list of provinces...");
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
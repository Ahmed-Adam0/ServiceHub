using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Authentication.Commands.RegisterCustomer;
using ServiceHub.Application.Features.Authentication.Commands.RegisterVendor;
using ServiceHub.Application.Features.Authentication.Queries.GetCurrentUser;
using ServiceHub.Application.Features.Authentication.DTOs;

namespace ServiceHub.API.Controllers;

/// <summary>
/// Handles authentication and registration requests.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR instance.</param>
    public AuthController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Registers a new customer user in the ServiceHub platform.
    /// </summary>
    /// <param name="command">The registration details payload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A response payload containing the registered user identifier.</returns>
    [HttpPost("register/customer")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterCustomer(
        [FromBody] RegisterCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return ProcessResult(result);
    }

    /// <summary>
    /// Registers a new vendor user in the ServiceHub platform.
    /// </summary>
    /// <param name="command">The registration details payload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A response payload containing the registered user identifier.</returns>
    [HttpPost("register/vendor")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterVendor(
        [FromBody] RegisterVendorCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return ProcessResult(result);
    }

    /// <summary>
    /// Retrieves current authenticated user context and profile details.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A response payload containing user profile details.</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(CurrentUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Unauthorized",
                Status = StatusCodes.Status401Unauthorized,
                Detail = "Authentication context did not return a valid user identity claim."
            });
        }

        var result = await _mediator.Send(new GetCurrentUserQuery(userId), cancellationToken);
        return ProcessResult(result);
    }

    /// <summary>
    /// Maps result entities to proper REST response code payloads.
    /// </summary>
    private IActionResult ProcessResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error is ValidationError validationError)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = validationError.Message,
                Instance = HttpContext.Request.Path
            };
            problemDetails.Extensions.Add("errors", validationError.Errors);
            return BadRequest(problemDetails);
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound => NotFound(new ProblemDetails
            {
                Title = "Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = result.Error.Message,
                Instance = HttpContext.Request.Path
            }),
            ErrorType.Conflict => Conflict(new ProblemDetails
            {
                Title = "Conflict",
                Status = StatusCodes.Status409Conflict,
                Detail = result.Error.Message,
                Instance = HttpContext.Request.Path
            }),
            ErrorType.Unauthorized => Unauthorized(new ProblemDetails
            {
                Title = "Unauthorized",
                Status = StatusCodes.Status401Unauthorized,
                Detail = result.Error.Message,
                Instance = HttpContext.Request.Path
            }),
            ErrorType.Forbidden => Forbid(),
            _ => BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Status = StatusCodes.Status400BadRequest,
                Detail = result.Error.Message,
                Instance = HttpContext.Request.Path
            })
        };
    }
}

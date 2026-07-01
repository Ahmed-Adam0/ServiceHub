using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Categories.DTOs;
using ServiceHub.Application.Features.Categories.Commands.CreateCategory;
using ServiceHub.Application.Features.Categories.Commands.UpdateCategory;
using ServiceHub.Application.Features.Categories.Commands.DeleteCategory;
using ServiceHub.Application.Features.Categories.Queries.GetCategoryById;
using ServiceHub.Application.Features.Categories.Queries.GetCategories;

namespace ServiceHub.API.Controllers;

/// <summary>
/// Handles marketplace category management operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoriesController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR instance.</param>
    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Creates a new marketplace category.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(
            request.Name,
            request.Slug,
            request.Description,
            request.Icon,
            request.DisplayOrder,
            request.ParentCategoryId);

        var result = await _mediator.Send(command, cancellationToken);
        return ProcessResult(result);
    }

    /// <summary>
    /// Updates an existing category details.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(
            id,
            request.Name,
            request.Slug,
            request.Description,
            request.Icon,
            request.DisplayOrder,
            request.ParentCategoryId);

        var result = await _mediator.Send(command, cancellationToken);
        return ProcessResult(result);
    }

    /// <summary>
    /// Deletes a category (soft-delete).
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
        return ProcessResult(result);
    }

    /// <summary>
    /// Retrieves a single category by its unique identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return ProcessResult(result);
    }

    /// <summary>
    /// Retrieves all active non-deleted categories.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoriesQuery(), cancellationToken);
        return ProcessResult(result);
    }

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

    private IActionResult ProcessResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok();
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

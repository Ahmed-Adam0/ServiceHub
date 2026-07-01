using System;
using MediatR;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Categories.DTOs;

namespace ServiceHub.Application.Features.Categories.Queries.GetCategoryById;

/// <summary>
/// Query record to retrieve a category details by unique ID.
/// </summary>
public record GetCategoryByIdQuery(Guid Id) : IRequest<Result<CategoryDto>>;

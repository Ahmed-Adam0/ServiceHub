using System.Collections.Generic;
using MediatR;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Categories.DTOs;

namespace ServiceHub.Application.Features.Categories.Queries.GetCategories;

/// <summary>
/// Query record to retrieve the complete list of active categories.
/// </summary>
public record GetCategoriesQuery() : IRequest<Result<IReadOnlyList<CategoryDto>>>;

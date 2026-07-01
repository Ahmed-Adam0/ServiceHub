using System;
using MediatR;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Categories.DTOs;

namespace ServiceHub.Application.Features.Categories.Commands.UpdateCategory;

/// <summary>
/// Command record to update an existing category details.
/// </summary>
public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string Slug,
    string Description,
    string Icon,
    int DisplayOrder,
    Guid? ParentCategoryId) : IRequest<Result<CategoryDto>>;

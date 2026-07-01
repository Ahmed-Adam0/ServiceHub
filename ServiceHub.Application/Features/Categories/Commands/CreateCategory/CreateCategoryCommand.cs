using System;
using MediatR;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Categories.DTOs;

namespace ServiceHub.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Command record to initiate the creation of a new category.
/// </summary>
public record CreateCategoryCommand(
    string Name,
    string Slug,
    string Description,
    string Icon,
    int DisplayOrder,
    Guid? ParentCategoryId) : IRequest<Result<CategoryDto>>;

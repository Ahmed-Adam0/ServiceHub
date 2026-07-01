using System;
using MediatR;
using ServiceHub.Application.Common.Models;

namespace ServiceHub.Application.Features.Categories.Commands.DeleteCategory;

/// <summary>
/// Command record to initiate the deletion of a category by unique ID.
/// </summary>
public record DeleteCategoryCommand(Guid Id) : IRequest<Result>;

using System;
using MediatR;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.Features.Authentication.DTOs;

namespace ServiceHub.Application.Features.Authentication.Queries.GetCurrentUser;

/// <summary>
/// Query record to retrieve the current user context and active Customer/Vendor profiles.
/// </summary>
public record GetCurrentUserQuery(Guid UserId) : IRequest<Result<CurrentUserDto>>;

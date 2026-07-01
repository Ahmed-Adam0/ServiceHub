using System;
using MediatR;
using ServiceHub.Application.Common.Models;

namespace ServiceHub.Application.Features.Authentication.Commands.RegisterVendor;

/// <summary>
/// Command record to register a new Vendor account and profile.
/// </summary>
public record RegisterVendorCommand(
    string Email,
    string Password,
    string FullName,
    string BusinessName,
    string DisplayName,
    string Bio,
    string? Logo,
    string? CoverImage) : IRequest<Result<Guid>>;

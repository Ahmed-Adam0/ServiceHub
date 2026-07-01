using System;
using MediatR;
using ServiceHub.Application.Common.Models;

namespace ServiceHub.Application.Features.Authentication.Commands.RegisterCustomer;

/// <summary>
/// Command record to register a new Customer account and profile.
/// </summary>
public record RegisterCustomerCommand(
    string Email,
    string Password,
    string FullName,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? Address,
    DateTime? DateOfBirth) : IRequest<Result<Guid>>;

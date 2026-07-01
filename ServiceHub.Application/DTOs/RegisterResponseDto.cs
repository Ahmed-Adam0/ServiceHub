using System;

namespace ServiceHub.Application.DTOs;

public class RegisterResponseDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string EmailConfirmationToken { get; set; } = null!;
}

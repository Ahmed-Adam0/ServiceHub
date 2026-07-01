using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServiceHub.Application.DTOs;
using ServiceHub.Application.Exceptions;
using ServiceHub.Application.IServices;
using ServiceHub.Domain.Entities;

namespace ServiceHub.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IMapper _mapper;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IMapper mapper,
        ILogger<IdentityService> logger)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        _logger.LogInformation("Attempting to register user with email: {Email}", request.Email);

        // 1. Business Rule: Email must be unique
        var existingEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingEmail != null)
        {
            _logger.LogWarning("Registration failed: Email {Email} already exists.", request.Email);
            throw new DuplicateEmailException(request.Email);
        }

        // 2. Business Rule: Username must be unique
        var existingUsername = await _userManager.FindByNameAsync(request.UserName);
        if (existingUsername != null)
        {
            _logger.LogWarning("Registration failed: Username {UserName} already exists.", request.UserName);
            throw new DuplicateUserNameException(request.UserName);
        }

        // 3. Instantiate ApplicationUser (Note: combining FirstName and LastName to FullName)
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FullName = $"{request.FirstName} {request.LastName}",
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = false // Created as unconfirmed
        };

        // 4. Create user (Identity hashes the password automatically)
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Registration failed with Identity validation errors for user {Email}", request.Email);
            throw new IdentityValidationException(result.Errors);
        }

        // 5. Business Rule: Automatically assign the Customer role
        const string customerRoleName = "Customer";
        var roleExists = await _roleManager.RoleExistsAsync(customerRoleName);
        if (!roleExists)
        {
            _logger.LogInformation("Role '{Role}' not found. Creating role.", customerRoleName);
            var roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>(customerRoleName));
            if (!roleResult.Succeeded)
            {
                _logger.LogError("Failed to create role '{Role}': {Errors}", customerRoleName, string.Join(", ", roleResult.Errors));
                throw new IdentityValidationException(roleResult.Errors);
            }
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(user, customerRoleName);
        if (!addToRoleResult.Succeeded)
        {
            _logger.LogError("Failed to assign role '{Role}' to user {Email}: {Errors}", customerRoleName, user.Email, string.Join(", ", addToRoleResult.Errors));
            throw new IdentityValidationException(addToRoleResult.Errors);
        }

        // 6. Generate Email Confirmation Token
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        _logger.LogInformation("User {Email} registered successfully. Generated confirmation token.", request.Email);

        // 7. Map and return response
        var response = _mapper.Map<RegisterResponseDto>(user);
        response.EmailConfirmationToken = token;

        return response;
    }
}

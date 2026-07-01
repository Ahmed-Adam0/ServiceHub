using System;
using System.Text;
using AutoMapper;
using FluentValidation;
using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ServiceHub.Application.Common.Interfaces;
using ServiceHub.Application.Common.Behaviors;
using ServiceHub.Application.Common.Models;
using ServiceHub.Application.IServices;
using ServiceHub.Application.Mapping;
using ServiceHub.Application.Options;
using ServiceHub.Domain.Entities;
using ServiceHub.Infrastructure.AppDbContext;
using ServiceHub.Infrastructure.Middleware;
using ServiceHub.Infrastructure.Services;

namespace ServiceHub.Infrastructure.ProgramService;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configure DbContext using SQL Server and ConnectionString
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // 2. Configure ASP.NET Core Identity
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            // Password Options
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout Options
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User Options
            options.User.RequireUniqueEmail = true;

            // Sign In Options
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // 3. Configure JWT Options
        var jwtSection = configuration.GetSection("JwtSettings");
        services.Configure<JwtOptions>(jwtSection);
        var jwtOptions = jwtSection.Get<JwtOptions>();

        if (jwtOptions == null || string.IsNullOrEmpty(jwtOptions.Secret))
        {
            throw new InvalidOperationException("JWT Settings are missing or invalid in appsettings.");
        }

        var key = Encoding.UTF8.GetBytes(jwtOptions.Secret);

        // 4. Configure Authentication & Authorization
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // Set to true in production
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        // 5. Configure HttpContext Accessor for Auditing
        services.AddHttpContextAccessor();

        // 6. Configure AutoMapper
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

        // 7. Configure FluentValidation Scanning
        services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

        // 8. Register Scoped Identity Service (Legacy support or feature transition path)
        services.AddScoped<IIdentityService, IdentityService>();

        // 9. Configure MediatR (Scanning Application Layer assembly containing 'Result')
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Result).Assembly));

        // 10. Register pipeline behaviors in execution order (Exception -> Logging -> Performance -> Validation)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // 11. CORS Named Policy Integration
        services.AddCors(options =>
        {
            options.AddPolicy("AngularPolicy", policy =>
            {
                policy.WithOrigins("https://localhost:4200", "https://127.0.0.1:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        // 12. Exception Handler & ProblemDetails Configuration
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // 13. Health Checks Integration
        services.AddHealthChecks()
                .AddSqlServer(connectionString ?? throw new InvalidOperationException("DefaultConnection connection string is missing."), name: "sqlserver");

        // 14. API Versioning Integration
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        // 15. Response Compression Integration
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
        });

        // 16. Localization Configuration
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { "en", "ar" };
            options.SetDefaultCulture(supportedCultures[0])
                   .AddSupportedCultures(supportedCultures)
                   .AddSupportedUICultures(supportedCultures);
        });

        return services;
    }
}

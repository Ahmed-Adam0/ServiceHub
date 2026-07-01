using Scalar.AspNetCore;
using Serilog;
using ServiceHub.Infrastructure.ProgramService;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();

// Configure OpenAPI with Title, Version and Security Scheme
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "ServiceHub API";
        document.Info.Version = "v1";

        // Add Bearer security scheme
        var securityScheme = new Microsoft.OpenApi.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter your JWT token: Bearer {token}"
        };

        document.Components ??= new Microsoft.OpenApi.OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, Microsoft.OpenApi.IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = securityScheme;

        // Require globally using OpenApiSecuritySchemeReference
        var securityRequirement = new Microsoft.OpenApi.OpenApiSecurityRequirement
        {
            [new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        };
        document.Security = new List<Microsoft.OpenApi.OpenApiSecurityRequirement> { securityRequirement };

        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(); // Enforce exception handler first to catch all downstream failures

app.UseSerilogRequestLogging();

app.UseRequestLocalization();

app.UseResponseCompression();

app.UseRouting(); // Explicitly register UseRouting before UseCors, UseAuthentication, and UseAuthorization

app.UseCors("AngularPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.MapOpenApi();
app.MapScalarApiReference();

app.Run();

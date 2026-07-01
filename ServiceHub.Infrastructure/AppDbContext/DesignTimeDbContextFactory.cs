using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ServiceHub.Infrastructure.AppDbContext;

namespace ServiceHub.Infrastructure.AppDbContext;

/// <summary>
/// Design-time factory for the <see cref="ApplicationDbContext"/>, allowing migrations scaffolding 
/// to bypass full API DI validations.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Try to read connection string from appsettings.json in the API project directory
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ServiceHub.API");
        if (!Directory.Exists(basePath))
        {
            basePath = Directory.GetCurrentDirectory(); // Fallback
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
                               ?? "Server=.\\SQLEXPRESS;Database=ServiceHubDb;Trusted_Connection=True;TrustServerCertificate=True;";

        builder.UseSqlServer(connectionString);

        return new ApplicationDbContext(builder.Options);
    }
}

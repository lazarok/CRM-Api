using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CRM.Persistence.Context.Auth;

public class AuthContextFactory : IDesignTimeDbContextFactory<AuthContext>
{
    public AuthContext CreateDbContext(string[] args)
    {
        // Get environment
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // Build config
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../CRM.Api"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        configuration = builder.Build();

        // Get connection string
        var optionsBuilder = new DbContextOptionsBuilder<AuthContext>();
        var connectionString = configuration.GetConnectionString("Auth");
        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(AuthContext).Assembly.FullName));
        return new AuthContext(optionsBuilder.Options);
    }
}
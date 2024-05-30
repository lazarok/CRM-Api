using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Persistence.Context.Crm;

public class CrmContextFactory : IDesignTimeDbContextFactory<CrmContext>
{
    public CrmContext CreateDbContext(string[] args)
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
        var optionsBuilder = new DbContextOptionsBuilder<CrmContext>();
        
        var connectionString = configuration.GetConnectionString("Crm")!;

        var slugTenant = configuration["SlugTenant"];

        connectionString = string.Format(connectionString, slugTenant);
        
        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(CrmContext).Assembly.FullName));
        
        var context = new CrmContext(optionsBuilder.Options);
        
        context.Database.EnsureCreated();
        
        return context;
    }
}
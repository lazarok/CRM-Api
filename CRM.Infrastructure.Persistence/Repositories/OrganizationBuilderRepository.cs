using CRM.Application.Repositories;
using CRM.Infrastructure.Persistence.Context.Crm;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Persistence.Repositories;

public class OrganizationBuilderRepository : IOrganizationBuilderRepository
{
    public async Task GenerateDataBase(string slugTenant)
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

        connectionString = string.Format(connectionString, slugTenant);
        
        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(CrmContext).Assembly.FullName));
        
        var context = new CrmContext(optionsBuilder.Options);
        
        await context.Database.EnsureCreatedAsync();
    }
}
using CRM.Application.Repositories;
using CRM.Application.Repositories.Common;
using CRM.Application.Services;
using CRM.Infrastructure.Persistence.Context.Auth;
using CRM.Infrastructure.Persistence.Context.Crm;
using CRM.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Infrastructure.Persistence;

public static class Startup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("Auth")));


        services.AddScoped<CrmContext>(provider =>
        {
            var workContext = provider.GetRequiredService<IWorkContext>();

            if (string.IsNullOrEmpty(workContext.SlugTenant))
            {
                throw new InvalidOperationException("SlugTenant not found");
            }
            
            var connectionString = configuration.GetConnectionString("Crm")!;

            connectionString = string.Format(connectionString, workContext.SlugTenant);
            var optionsBuilder = new DbContextOptionsBuilder<CrmContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new CrmContext(optionsBuilder.Options);
        });

        services.AddScoped<IOrganizationBuilderRepository, OrganizationBuilderRepository>();
        
        services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
        services.AddScoped<ICrmUnitOfWork, CrmUnitOfWork>();

        return services;
    }
}
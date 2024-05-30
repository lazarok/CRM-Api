using CRM.Application.Repositories.Common;
using CRM.Persistence.Context.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Persistence;

public static class Startup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("Auth"),
                b => b.MigrationsAssembly(typeof(AuthContext).Assembly.FullName)));
        
        services.AddScoped<IUnitOfWork, AuthUnitOfWork>();
        
        
        // Seed
        
        // var container = services.BuildServiceProvider();
        // var unitOfWork = container.GetRequiredService<IUnitOfWork>();
        //
        // DefaultUsers.SeedAsync(unitOfWork);

        return services;
    }
}
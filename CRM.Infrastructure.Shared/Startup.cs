using CRM.Application.Services;
using CRM.Infrastructure.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Infrastructure.Shared;

public static class Startup
{
    public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITokenService, TokenService>();           
        services.AddTransient<IProductPictureService, ProductPictureService>();
        
        services.AddSingleton<IUriService>(provider => {
            var accesor = provider.GetRequiredService<IHttpContextAccessor>();
            var request = accesor.HttpContext!.Request;
            var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
            return new UriService(absoluteUri);
        });
    }
}
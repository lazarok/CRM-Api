using CRM.Application.Services;

namespace CRM.Api.Middlewares;

public class UserIdentityMiddleware
{
    private readonly RequestDelegate _next;

    public UserIdentityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext? context, IWorkContext workContext, IConfiguration configuration)
    {
        var slugTenantRoute = context?.GetRouteValue("slugTenant")?.ToString();
        if (slugTenantRoute is null)
        { 
            await _next(context);
            return;
        }
        
        workContext.SlugTenant = slugTenantRoute;
        await _next(context);
    }
}
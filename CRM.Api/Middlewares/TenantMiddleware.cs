using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using CRM.Application.Models;
using CRM.Application.Services;
using CRM.Infrastructure.Persistence.Context.Auth;
using Microsoft.EntityFrameworkCore;

namespace CRM.Api.Middlewares;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext? context, IWorkContext workContext, IConfiguration configuration, AuthContext authContext)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                DictionaryKeyPolicy = null,
                PropertyNamingPolicy = null
            };
            
            var tenant = context.User.FindFirst("Tenant")?.Value;
            if (workContext.SlugTenant is not null && tenant != workContext.SlugTenant)
            {
                var response = ApiResponse.Error(ResponseCode.Forbidden, "Route Tenant slug does not match access token");
                
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                
                await context.Response.WriteAsJsonAsync(response, jsonSerializerOptions);
                return;
            }
            
            long.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
            long.TryParse(context.User.FindFirst("OrganizationId")?.Value, out var organizationId);
            
            var user = await authContext.Users
                .Include(x => x.Organization)
                .FirstOrDefaultAsync(user => user.Id == userId);

            if (user?.Organization.SlugTenant is not null && user?.Organization.SlugTenant != tenant)
            {
                var response = ApiResponse.Error(ResponseCode.Forbidden, "Route Tenant slug does not match Organization");
                
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                
                await context.Response.WriteAsJsonAsync(response, jsonSerializerOptions);
                return;
            }
            
            workContext.OrganizationId = organizationId;
            workContext.UserId = userId;
        }
        
        await _next(context);
    }
}
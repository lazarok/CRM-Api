using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using CRM.Application.Models;
using CRM.Application.Services;

namespace CRM.Api.Middlewares;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext? context, IWorkContext workContext, IConfiguration configuration)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var tenant = context.User.FindFirst("Tenant")?.Value;
            if (tenant != workContext.SlugTenant)
            {
                var response = ApiResponse.Error(ResponseCode.Forbidden, "Route Tenant slug does not match access token");
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    DictionaryKeyPolicy = null,
                    PropertyNamingPolicy = null
                };
                
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                
                await context.Response.WriteAsJsonAsync(response, jsonSerializerOptions);
                return;
            }
        }
        
        await _next(context);
    }
}
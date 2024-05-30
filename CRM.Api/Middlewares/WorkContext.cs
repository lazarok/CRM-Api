using CRM.Application.Services;

namespace CRM.Api.Middlewares;

public class WorkContext : IWorkContext
{
    public string? UserId { get; set; }
    public string? SlugTenant { get; set;}
}
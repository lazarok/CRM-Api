using CRM.Application.Services;

namespace CRM.Api.Middlewares;

public class WorkContext : IWorkContext
{
    public long? UserId { get; set; }
    public string? SlugTenant { get; set;}
    public long? OrganizationId { get; set; }
}
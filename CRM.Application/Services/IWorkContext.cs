namespace CRM.Application.Services;

public interface IWorkContext
{
    /// <summary>
    /// User Id
    /// </summary>
    public long? UserId { get; set; }
    
    /// <summary>
    /// Slug tenant
    /// </summary>
    public string? SlugTenant { get; set; }
    
    public long? OrganizationId { get; set; }
}
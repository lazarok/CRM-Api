namespace CRM.Application.Services;

public interface IWorkContext
{
    /// <summary>
    /// User Id
    /// </summary>
    public string? UserId { get; set; }
    
    /// <summary>
    /// Slug tenant
    /// </summary>
    public string? SlugTenant { get; set; }
}
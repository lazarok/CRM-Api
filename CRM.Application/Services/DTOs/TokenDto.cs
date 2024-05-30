namespace CRM.Application.Services.DTOs;

public class TokenDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    
    public string SlugTenant { get; set; }
}
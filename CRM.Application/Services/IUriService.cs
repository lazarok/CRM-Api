namespace CRM.Application.Services;

public interface IUriService
{
    Uri GetBaseUri(string actionUrl);
}
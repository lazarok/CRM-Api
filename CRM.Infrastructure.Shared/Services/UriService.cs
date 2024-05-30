using CRM.Application.Services;

namespace CRM.Infrastructure.Shared.Services;

public class UriService : IUriService
{
    private readonly string _baseUri;

    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }

    public Uri GetBaseUri(string actionUrl)
    {
        string baseUrl = $"{_baseUri}{actionUrl}";

        return new Uri(baseUrl);
    }
}
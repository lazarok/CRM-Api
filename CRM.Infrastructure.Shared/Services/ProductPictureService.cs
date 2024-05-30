using CRM.Application.Services;
using CRM.Infrastructure.Shared.Services.Common;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Shared.Services;

public class ProductPictureService : PictureService, IProductPictureService
{
    public ProductPictureService(IConfiguration configuration, IUriService uriService) : base(configuration, uriService)
    {
        
    }
}
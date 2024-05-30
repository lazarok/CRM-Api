using CRM.Application.Features.Products.DTOs;
using CRM.Application.Helpers.Mapping;
using CRM.Domain.Entities;

namespace CRM.Application.Features.Products;

public class ProductMapping : IMapping
{
    public void CreateMap(MappingProfile profile)
    {
        profile.CreateMap<Product, ProductDto>();
    }
}
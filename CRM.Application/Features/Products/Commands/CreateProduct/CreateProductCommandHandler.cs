using CRM.Application.Features.Products.DTOs;
using CRM.Application.Models;
using CRM.Application.Repositories.Common;
using CRM.Domain.Entities;
using MediatR;

namespace CRM.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<ProductDto>>
{
    private readonly ICrmUnitOfWork _crmUnitOfWork;

    public CreateProductCommandHandler(
        ICrmUnitOfWork crmUnitOfWork)
    {
        _crmUnitOfWork = crmUnitOfWork;
    }
    
    public async Task<ApiResponse<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            ProductStock = request.ProductStock,
            Price = request.Price
        };
        
        _crmUnitOfWork.Repository<Product>().Add(product);

        await _crmUnitOfWork.SaveAsync(cancellationToken);

        return ApiResponse.OkMapped<ProductDto>(product);
    }
}
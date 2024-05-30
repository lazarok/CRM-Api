using CRM.Application.Features.Products.DTOs;
using CRM.Application.Models;
using CRM.Application.Repositories.Common;
using CRM.Application.Services;
using CRM.Domain.Entities;
using MediatR;

namespace CRM.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<ProductDto>>
{
    private readonly ICrmUnitOfWork _crmUnitOfWork;
    private readonly IWorkContext _workContext;

    public CreateProductCommandHandler(
        ICrmUnitOfWork crmUnitOfWork,
        IWorkContext workContext)
    {
        _crmUnitOfWork = crmUnitOfWork;
        _workContext = workContext;
    }
    
    public async Task<ApiResponse<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();
        
        if (await _crmUnitOfWork.Repository<Product>().AnyAsync(x => x.Name == name, cancellationToken))
            return ApiResponse.Error<ProductDto>(ResponseCode.Found, "Exist product");
        
        var product = new Product
        {
            Name = name,
            Description = request.Description,
            ProductStock = request.ProductStock,
            Price = request.Price,
            CreatedBy = _workContext.UserId ?? throw new ArgumentException("workContext UserId")
        };
        
        _crmUnitOfWork.Repository<Product>().Add(product);

        await _crmUnitOfWork.SaveAsync(cancellationToken);

        return ApiResponse.OkMapped<ProductDto>(product, ResponseCode.Created);
    }
}
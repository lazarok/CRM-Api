using CRM.Application.Features.Products.DTOs;
using CRM.Application.Models;
using CRM.Application.Repositories.Common;
using CRM.Application.Services;
using CRM.Domain.Entities;
using MediatR;

namespace CRM.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse<ProductDto>>
{
    private readonly ICrmUnitOfWork _crmUnitOfWork;

    public UpdateProductCommandHandler(
        ICrmUnitOfWork crmUnitOfWork,
        IWorkContext workContext)
    {
        _crmUnitOfWork = crmUnitOfWork;
    }
    
    public async Task<ApiResponse<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var repository = _crmUnitOfWork.Repository<Product>();

        var product = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if(product is null)
            return ApiResponse.Error<ProductDto>(ResponseCode.NotFound, "Product not found");
        
        var name = request.Name.Trim();
        
        if (await repository.AnyAsync(x => x.Name == name && x.Id != product.Id, cancellationToken))
            return ApiResponse.Error<ProductDto>(ResponseCode.Found, "Name not available");

        product.Name = name;
        product.Description = request.Description;
        product.ProductStock = request.ProductStock;
        product.Price = request.Price;
        
        repository.Update(product);

        await _crmUnitOfWork.SaveAsync(cancellationToken);

        return ApiResponse.OkMapped<ProductDto>(product);
    }
}
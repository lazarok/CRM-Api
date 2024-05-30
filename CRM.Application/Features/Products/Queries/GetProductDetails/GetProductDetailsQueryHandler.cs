using CRM.Application.Features.Products.DTOs;
using CRM.Application.Models;
using CRM.Application.Repositories.Common;
using CRM.Domain.Entities;
using MediatR;

namespace CRM.Application.Features.Products.Queries.GetProductDetails;

public class GetProductDetailsQueryHandler : IRequestHandler<GetProductDetailsQuery, ApiResponse<ProductDto>>
{
    private readonly ICrmUnitOfWork _crmUnitOfWork;

    public GetProductDetailsQueryHandler(ICrmUnitOfWork crmUnitOfWork)
    {
        _crmUnitOfWork = crmUnitOfWork;
    }

    public async Task<ApiResponse<ProductDto>> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
    {
        var repository = _crmUnitOfWork.Repository<Product>();

        var product = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if(product is null)
            return ApiResponse.Error<ProductDto>(ResponseCode.NotFound, "Product not found");
        
        return ApiResponse.OkMapped<ProductDto>(product);
    }
}
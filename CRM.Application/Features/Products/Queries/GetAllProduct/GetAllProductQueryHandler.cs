using CRM.Application.Extensions;
using CRM.Application.Features.Products.DTOs;
using CRM.Application.Models;
using CRM.Application.Repositories.Common;
using CRM.Domain.Entities;
using MediatR;

namespace CRM.Application.Features.Products.Queries.GetAllProduct;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, ApiResponse<PagedList<ProductDto>>>
{
    private readonly ICrmUnitOfWork _crmUnitOfWork;

    public GetAllProductQueryHandler(ICrmUnitOfWork crmUnitOfWork)
    {
        _crmUnitOfWork = crmUnitOfWork;
    }

    public async Task<ApiResponse<PagedList<ProductDto>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        var search = request.Search?.Trim().ToLower();
        
        var queryable = _crmUnitOfWork.Repository<Product>().GetAll();

        if (!string.IsNullOrEmpty(search))
            queryable = queryable.Where(x => x.Name.Contains(search) || (x.Description ?? string.Empty).Contains(search));
        
        var pagedList = await queryable.ToPagedListAsync(request);
        
        return ApiResponse.OkMapped<PagedList<ProductDto>>(pagedList);
    }
}
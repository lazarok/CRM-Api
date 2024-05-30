using CRM.Application.Features.Products.DTOs;
using CRM.Application.Models;
using CRM.Application.Repositories.Common;
using CRM.Domain.Entities;
using MediatR;

namespace CRM.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApiResponse>
{
    private readonly ICrmUnitOfWork _crmUnitOfWork;

    public DeleteProductCommandHandler(
        ICrmUnitOfWork crmUnitOfWork)
    {
        _crmUnitOfWork = crmUnitOfWork;
    }
    
    public async Task<ApiResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var repository = _crmUnitOfWork.Repository<Product>();

        var product = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if(product is null)
            return ApiResponse.Error(ResponseCode.NotFound, "Product not found");
        
        repository.Remove(product);

        await _crmUnitOfWork.SaveAsync(cancellationToken);

        return ApiResponse.Ok();
    }
}
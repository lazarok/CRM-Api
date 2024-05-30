using CRM.Application.Features.Products.DTOs;
using CRM.Application.Models;
using MediatR;

namespace CRM.Application.Features.Products.Queries.GetProductDetails;

public record GetProductDetailsQuery(long Id) : IRequest<ApiResponse<ProductDto>> {}
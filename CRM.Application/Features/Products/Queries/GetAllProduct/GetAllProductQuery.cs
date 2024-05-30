using CRM.Application.Features.Products.DTOs;
using CRM.Application.Models;
using MediatR;

namespace CRM.Application.Features.Products.Queries.GetAllProduct;

public class GetAllProductQuery : PaginationFilter, IRequest<ApiResponse<PagedList<ProductDto>>> {}
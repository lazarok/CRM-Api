using CRM.Application.Features.Products.DTOs;
using CRM.Application.Models;
using MediatR;

namespace CRM.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<ApiResponse<ProductDto>>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int ProductStock { get; set; }
    public decimal Price { get; set; }
}
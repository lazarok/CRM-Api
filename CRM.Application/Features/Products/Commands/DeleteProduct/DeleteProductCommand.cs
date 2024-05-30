using CRM.Application.Models;
using MediatR;

namespace CRM.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(long Id) : IRequest<ApiResponse>;
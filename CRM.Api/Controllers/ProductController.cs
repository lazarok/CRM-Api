using AutoMapper;
using CRM.Api.Controllers.Base;
using CRM.Application.Features.Products.Commands.CreateProduct;
using CRM.Application.Features.Products.Commands.DeleteProduct;
using CRM.Application.Features.Products.Commands.UpdateProduct;
using CRM.Application.Features.Products.DTOs;
using CRM.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Controllers;

[Authorize]
[Route("api/{slugTenant}/products")]
public class ProductController : BaseApiController
{
    private readonly IMapper _mapper;

    public ProductController(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Create product
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request)
    {
        return BuildResponse(await Mediator.Send(request));
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct(long productId, [FromBody] UpdateProductRequest request)
    {
        var command = _mapper.Map<UpdateProductCommand>(request);
        command.Id = productId;
        return BuildResponse(await Mediator.Send(command));
    }
    
    /// <summary>
    /// Delete product
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(long productId)
    {
        return BuildResponse(await Mediator.Send(new DeleteProductCommand(productId)));
    }
}
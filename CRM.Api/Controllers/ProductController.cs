using CRM.Api.Controllers.Base;
using CRM.Application.Features.Products.Commands.CreateProduct;
using CRM.Application.Features.Products.DTOs;
using CRM.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Controllers;

[Authorize]
[Route("api/{slugTenant}/products")]
public class ProductController : BaseApiController
{

    /// <summary>
    /// Create product
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request)
    {
        return BuildResponse(await Mediator.Send(request));
    }
}
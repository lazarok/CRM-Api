using CRM.Api.Controllers.Base;
using CRM.Application.Features.Register.Commands.RegisterAdmin;
using CRM.Application.Features.Register.Commands.RegisterUser;
using CRM.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Controllers;

[Route("api/register")]
public class RegisterController : BaseApiController
{

    /// <summary>
    /// Register admin
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [HttpPost("admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminCommand request)
    {
        return BuildResponse(await Mediator.Send(request));
    }
    
    /// <summary>
    /// Register user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [HttpPost("user")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand request)
    {
        return BuildResponse(await Mediator.Send(request));
    }
}
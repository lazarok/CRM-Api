using CRM.Api.Controllers.Base;
using CRM.Application.Fetures.Auth.RegisterAdmin;
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
    [HttpPost("register-admin")]
    public async Task<IActionResult> Register([FromBody] RegisterAdminCommand request)
    {
        return BuildResponse(await Mediator.Send(request));
    }
}
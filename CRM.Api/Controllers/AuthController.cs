using CRM.Api.Controllers.Base;
using CRM.Application.Features.Auth.Commands.Login;
using CRM.Application.Features.Auth.Commands.RefreshToken;
using CRM.Application.Models;
using CRM.Application.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Controllers;

[Route("api/auth")]
public class AuthController : BaseApiController
{

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse<TokenDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TokenDto>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<TokenDto>), StatusCodes.Status403Forbidden)]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        return BuildResponse(await Mediator.Send(request));
    }
    
    /// <summary>
    /// Refresh token
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse<TokenDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TokenDto>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<TokenDto>), StatusCodes.Status403Forbidden)]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> Login([FromBody] RefreshTokenCommand request)
    {
        return BuildResponse(await Mediator.Send(request));
    }
}
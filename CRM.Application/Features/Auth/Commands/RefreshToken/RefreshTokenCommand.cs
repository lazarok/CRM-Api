using CRM.Application.Models;
using CRM.Application.Services.DTOs;
using MediatR;

namespace CRM.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<ApiResponse<TokenDto>>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
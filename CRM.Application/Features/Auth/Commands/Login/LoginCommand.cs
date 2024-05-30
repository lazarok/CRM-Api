using CRM.Application.Models;
using CRM.Application.Services.DTOs;
using MediatR;

namespace CRM.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<ApiResponse<TokenDto>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
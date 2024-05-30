using CRM.Application.Models;
using MediatR;

namespace CRM.Application.Features.Register.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<ApiResponse>
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public int OrganizationId { get; set; }
}
using CRM.Application.Models;
using MediatR;

namespace CRM.Application.Fetures.Auth.RegisterAdmin;

public class RegisterAdminCommand : IRequest<ApiResponse>
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string OrganizationName { get; set; }
}
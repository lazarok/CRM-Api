using CRM.Application.Helpers;
using CRM.Application.Models;
using CRM.Application.Repositories.Common;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using MediatR;

namespace CRM.Application.Features.Register.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApiResponse>
{
    private readonly IAuthUnitOfWork _authUnitOfWork;

    public RegisterUserCommandHandler(
        IAuthUnitOfWork authUnitOfWork)
    {
        _authUnitOfWork = authUnitOfWork;
    }

    public async Task<ApiResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userRepository = _authUnitOfWork.Repository<User>();

        var email = request.Email.Trim().ToLower();

        if (await userRepository.AnyAsync(x => x.Email.ToLower() == email, cancellationToken))
            return ApiResponse.Error(ResponseCode.Found, "Exist user");

        var organizationRepository = _authUnitOfWork.Repository<Organization>();

        var organization = await organizationRepository.FirstOrDefaultAsync(x => x.SlugTenant == request.SlugTenant, cancellationToken: cancellationToken);
        if (organization is null)
            return ApiResponse.Error(ResponseCode.NotFound, "Organization not found");
        
        var newUser = new User
        {
            Email = email,
            Name = request.Name.Trim(),
            Role = UserRole.Employed,
            OrganizationId = organization.Id,
            PasswordHash = PasswordHelper.CreatePasswordHash(request.Password),
            RefreshToken = null,
            RefreshTokenExpires = null
        };
        
        userRepository.Add(newUser);

        await _authUnitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok(ResponseCode.Created, message: "");
    }
}
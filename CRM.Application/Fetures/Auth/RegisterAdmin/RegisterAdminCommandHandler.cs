using CRM.Application.Extensions;
using CRM.Application.Helpers;
using CRM.Application.Models;
using CRM.Application.NotificationBus;
using CRM.Application.Repositories.Common;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using MediatR;

namespace CRM.Application.Fetures.Auth.RegisterAdmin;

public class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand, ApiResponse>
{
    private readonly IAuthUnitOfWork _authUnitOfWork;
    private readonly IMediator _mediator;

    public RegisterAdminCommandHandler(
        IAuthUnitOfWork authUnitOfWork, 
        IMediator mediator)
    {
        _authUnitOfWork = authUnitOfWork;
        _mediator = mediator;
    }
    
    public async Task<ApiResponse> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        var userRepository = _authUnitOfWork.Repository<User>();
        
        var email = request.Email.Trim().ToLower();
        
        if (await userRepository.AnyAsync(x => x.Email.ToLower() == email, cancellationToken))
            return ApiResponse.Error(ResponseCode.Found, "Exist user");
        
        var organizationRepository = _authUnitOfWork.Repository<Organization>();
        
        var orgName = request.OrganizationName.Trim();
        var slugTenant = orgName.GenerateSlug();
        
        if (await organizationRepository.AnyAsync(x => x.Name == orgName, cancellationToken))
            return ApiResponse.Error(ResponseCode.Found, "Exist organization");

        var countSlug = await organizationRepository.CountAsync(x => x.SlugTenant == slugTenant, cancellationToken);
        if (countSlug > 0)
        {
            slugTenant = $"{slugTenant}-{countSlug.ToString()}";
        }

        await _authUnitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var newOrganization = new Organization
            {
                Name = orgName,
                SlugTenant = slugTenant,
            };
            organizationRepository.Add(newOrganization);
            await _authUnitOfWork.SaveChangesAsync(cancellationToken);
            
            var newUser = new User
            {
                Email = email,
                Name = request.Name.Trim(),
                Role = UserRole.Admin,
                OrganizationId = newOrganization.Id,
                PasswordHash = PasswordHelper.CreatePasswordHash(request.Password),
                RefreshToken = null,
                RefreshTokenExpires = null
            };
            userRepository.Add(newUser);

            await _authUnitOfWork.SaveChangesAsync(cancellationToken);
            
            await _authUnitOfWork.CommitTransactionAsync(cancellationToken);
            
            // enqueue generate Product DB for slug tenant
            await _mediator.Publish(new OrganizationCreated
            {
                SlugTenant = slugTenant
            }, cancellationToken);

            return ApiResponse.Ok(ResponseCode.Created, "Wait a few seconds while the organization is created");
        }
        catch (Exception e)
        {
            // TODO: add logg
            await _authUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse.Error(ResponseCode.Unhandled);
        }
    }
}
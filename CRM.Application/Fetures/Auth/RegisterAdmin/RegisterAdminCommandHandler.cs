using CRM.Application.Extensions;
using CRM.Application.Models;
using CRM.Application.Repositories.Common;
using CRM.Application.ServiceBus;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using MediatR;

namespace CRM.Application.Fetures.Auth.RegisterAdmin;

public class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand, ApiResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public RegisterAdminCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    
    public async Task<ApiResponse> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.Repository<User>();
        
        var email = request.Email.Trim().ToLower();
        
        var user = await userRepository.FirstOrDefaultAsync(x => 
            string.Equals(x.Email.ToLower(), email, StringComparison.Ordinal), 
            cancellationToken);

        if (user is not null)
            return ApiResponse.Error(ResponseCode.Found, "Exist user");

        
        var organizationRepository = _unitOfWork.Repository<Organization>();
        
        var orgName = request.Name.Trim();
        var slug = orgName.GenerateSlug();

        var organization = await organizationRepository
            .FirstOrDefaultAsync(x => x.Name == orgName || x.SlugTenant == slug, cancellationToken);
        if (organization is not null)
            return ApiResponse.Error(ResponseCode.Found, "Exist organization");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var newOrganization = new Organization
            {
                Name = orgName,
                SlugTenant = slug,
            };
            organizationRepository.Add(newOrganization);
            //await _unitOfWork.SaveChangesAsync(cancellationToken);

            var newUser = new User
            {
                Email = email,
                Name = request.Name.Trim(),
                Role = UserRole.Admin,
                OrganizationId = newOrganization.Id,
                PasswordHash = null,
                PasswordSalt = null,
                RefreshToken = null,
                RefreshTokenExpires = null
            };
            userRepository.Add(newUser);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            
            // enqueue gennerate Product DB for slug tenant
            await _mediator.Publish(new OrganizationCreated()
            {

            });
            
            return ApiResponse.Ok();
        }
        catch (Exception e)
        {
            // add logs
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse.Error(ResponseCode.Unhandled);
        }
    }
}
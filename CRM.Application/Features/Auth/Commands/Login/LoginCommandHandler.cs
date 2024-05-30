using System.Net;
using System.Security.Claims;
using CRM.Application.Exceptions;
using CRM.Application.Helpers;
using CRM.Application.Models;
using CRM.Application.Repositories.Common;
using CRM.Application.Services;
using CRM.Application.Services.DTOs;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace CRM.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<TokenDto>>
{
    private readonly IAuthUnitOfWork _authUnitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(
        IAuthUnitOfWork authUnitOfWork, 
        ITokenService tokenService, 
        IConfiguration configuration)
    {
        _authUnitOfWork = authUnitOfWork;
        _tokenService = tokenService;
        _configuration = configuration;
    }
    public async Task<ApiResponse<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userRepository = _authUnitOfWork.Repository<User>();
        
        var email = request.Email.Trim();
        
        var user = await userRepository
            .FirstOrDefaultAsync(x => 
                x.Email == email, 
                eagerIncludes: [nameof(User.Organization)],
                cancellationToken: cancellationToken);
        
        if (user is null)
        {
            return ApiResponse.Error<TokenDto>(ResponseCode.BadRequest, "Invalid email or password");
        }

        if (!PasswordHelper.VerifyPasswordHash(request.Password, user.PasswordHash!))
        {
            return ApiResponse.Error<TokenDto>(ResponseCode.BadRequest, "Invalid email or password");
        }
        
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("Tenant", user.Organization.SlugTenant),
            new Claim("OrganizationId", user.OrganizationId.ToString())
        };

        var token = _tokenService.BuildToken(claims)!;

        token.SlugTenant = user.Organization.SlugTenant;
        
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpires = DateTime.UtcNow.AddMonths(int.Parse(_configuration["Jwt:RefreshTokenExpires"]!));

        userRepository.Update(user);
        
        await _authUnitOfWork.SaveAsync(cancellationToken);

        return ApiResponse.Ok(token);
    }
}
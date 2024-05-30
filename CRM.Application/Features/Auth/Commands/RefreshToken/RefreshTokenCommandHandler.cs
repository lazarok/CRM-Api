using System.Security.Claims;
using CRM.Application.Extensions;
using CRM.Application.Models;
using CRM.Application.Repositories.Common;
using CRM.Application.Services;
using CRM.Application.Services.DTOs;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace CRM.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ApiResponse<TokenDto>>
{
    private readonly IAuthUnitOfWork _authUnitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public RefreshTokenCommandHandler(
        IAuthUnitOfWork authUnitOfWork, 
        ITokenService tokenService, 
        IConfiguration configuration)
    {
        _authUnitOfWork = authUnitOfWork;
        _tokenService = tokenService;
        _configuration = configuration;
    }
    public async Task<ApiResponse<TokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userRepository = _authUnitOfWork.Repository<User>();
        
        var userId = _tokenService
            .GetClaims(request.AccessToken)
            .GetClaimValue<int>(ClaimTypes.NameIdentifier);
        
        var user = await userRepository.GetByIdAsync(
            userId, 
            eagerIncludes: [nameof(User.Organization)],
            cancellationToken: cancellationToken);
        
        if (user is null)
        {
            return ApiResponse.Error<TokenDto>(ResponseCode.NotFound, "User not found");
        }
        
        if (string.IsNullOrEmpty(user.RefreshToken) || user.RefreshToken != request.RefreshToken)
        {
            return ApiResponse.Error<TokenDto>(ResponseCode.Unauthorized, "Invalid refresh token!");
        }
        
        if (user.RefreshTokenExpires == null || user.RefreshTokenExpires <= DateTime.UtcNow)
        {
            return ApiResponse.Error<TokenDto>(ResponseCode.Unauthorized, "Refresh token expired");
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
        
        await _authUnitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok(token);
    }
}
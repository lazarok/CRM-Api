using System.Security.Claims;
using CRM.Application.Services.DTOs;

namespace CRM.Application.Services;

public interface ITokenService
{
    TokenDto BuildToken(IEnumerable<Claim> claims, DateTime? expires = null);
    IEnumerable<Claim> GetClaims(string token);
}
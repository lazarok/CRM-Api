using CRM.Domain.Common;
using CRM.Domain.Enums;

namespace CRM.Domain.Entities;

public class User : BaseEntity
{
    public long OrganizationId { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required UserRole Role { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }

    public Organization Organization { get; set; } = default!;
}
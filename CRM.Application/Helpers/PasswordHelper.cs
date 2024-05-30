using BC = BCrypt.Net.BCrypt;

namespace CRM.Application.Helpers;

public static class PasswordHelper
{
    public static string CreatePasswordHash(string password)
    {
        return BC.HashPassword(password);
    }

    public static bool VerifyPasswordHash(string password, string passwordHash)
    {
        return BC.Verify(password, passwordHash);
    }
}
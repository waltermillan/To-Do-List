using System.Security.Cryptography;
using System.Text;

namespace Core.Helpers;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashBytes);
        }
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        var enteredPasswordHash = HashPassword(enteredPassword);
        return enteredPasswordHash == storedHash;
    }
}

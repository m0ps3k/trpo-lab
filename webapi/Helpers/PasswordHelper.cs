using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TaskApi.Helpers;

public static class PasswordHelper
{
    public static byte[] GetSecureSalt() => RandomNumberGenerator.GetBytes(32);

    public static string HashUsingPbkdf2(string password, byte[] salt) =>
        Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 300000, 32));
}

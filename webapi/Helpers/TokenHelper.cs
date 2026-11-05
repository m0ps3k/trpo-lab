using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace TaskApi.Helpers;

public static class TokenHelper
{
    public const string Issuer = "https://sutkt.ru/";
    public const string Audience = "https://sutkt.ru/";
    public const string Secret = "FgAfb4lY2aSX0nv8t/F3GZFq0mqDU9O4lglnLMZankkggIaxrZRLeDzSdTEWgICAMVuL5bTKthb+BZJE9fPV6Q==";

    public static Task<string> GenerateAccessToken(int userId)
    {
        var handler = new JwtSecurityTokenHandler();
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, userId.ToString())]),
            Issuer = Issuer, Audience = Audience, Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Convert.FromBase64String(Secret)), SecurityAlgorithms.HmacSha256Signature)
        };
        return Task.FromResult(handler.WriteToken(handler.CreateToken(descriptor)));
    }

    public static Task<string> GenerateRefreshToken() =>
        Task.FromResult(Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)));
}

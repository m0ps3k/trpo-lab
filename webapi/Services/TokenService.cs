using Microsoft.EntityFrameworkCore;
using TaskApi.Data.Entities;
using TaskApi.Data.Requests;
using TaskApi.Data.Responses;
using TaskApi.Helpers;
using TaskApi.Interfaces;

namespace TaskApi.Services;

public class TokenService(TasksDbContext tasksDbContext) : ITokenService
{
    public async Task<Tuple<string, string>?> GenerateTokensAsync(int userId)
    {
        var user = await tasksDbContext.Users.Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) return null;

        var accessToken = await TokenHelper.GenerateAccessToken(userId);
        var refreshToken = await TokenHelper.GenerateRefreshToken();
        var salt = PasswordHelper.GetSecureSalt();
        var hash = PasswordHelper.HashUsingPbkdf2(refreshToken, salt);
        await RemoveRefreshTokenAsync(user);
        tasksDbContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = userId, Ts = DateTime.Now, ExpiryDate = DateTime.Now.AddDays(20),
            TokenHash = hash, TokenSalt = Convert.ToBase64String(salt)
        });
        await tasksDbContext.SaveChangesAsync();
        return Tuple.Create(accessToken, refreshToken);
    }

    public async Task<bool> RemoveRefreshTokenAsync(User user)
    {
        var record = await tasksDbContext.Users.Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == user.Id);
        if (record is null || !record.RefreshTokens.Any()) return false;
        tasksDbContext.RefreshTokens.RemoveRange(record.RefreshTokens);
        return true;
    }

    public async Task<ValidateRefreshTokenResponse> ValidateRefreshTokenAsync(RefreshTokenRequest request)
    {
        var token = await tasksDbContext.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == request.UserId);
        if (token is null)
            return new() { Error = "Invalid session or user is already logged out", ErrorCode = "R02" };
        var hash = PasswordHelper.HashUsingPbkdf2(request.RefreshToken, Convert.FromBase64String(token.TokenSalt));
        if (token.TokenHash != hash)
            return new() { Error = "Invalid refresh token", ErrorCode = "R03" };
        if (token.ExpiryDate < DateTime.Now)
            return new() { Error = "Refresh token has expired", ErrorCode = "R04" };
        return new() { Success = true, UserId = token.UserId };
    }
}

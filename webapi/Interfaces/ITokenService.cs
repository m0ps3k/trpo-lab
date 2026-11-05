using TaskApi.Data.Entities;
using TaskApi.Data.Requests;
using TaskApi.Data.Responses;

namespace TaskApi.Interfaces;

public interface ITokenService
{
    Task<Tuple<string, string>?> GenerateTokensAsync(int userId);
    Task<ValidateRefreshTokenResponse> ValidateRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    Task<bool> RemoveRefreshTokenAsync(User user);
}

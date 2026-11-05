using Microsoft.EntityFrameworkCore;
using TaskApi.Data.Entities;
using TaskApi.Data.Requests;
using TaskApi.Data.Responses;
using TaskApi.Helpers;
using TaskApi.Interfaces;

namespace TaskApi.Services;

public class UserService(TasksDbContext tasksDbContext, ITokenService tokenService) : IUserService
{
    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        var user = await tasksDbContext.Users.SingleOrDefaultAsync(u => u.IsActive && u.Email == request.Email);
        if (user is null)
            return new() { Error = "Email not found", ErrorCode = "L02" };
        var hash = PasswordHelper.HashUsingPbkdf2(request.Password, Convert.FromBase64String(user.PasswordSalt));
        if (user.Password != hash)
            return new() { Error = "Invalid Password", ErrorCode = "L03" };
        var tokens = await tokenService.GenerateTokensAsync(user.Id);
        return new() { Success = true, AccessToken = tokens!.Item1, RefreshToken = tokens.Item2 };
    }

    public async Task<LogoutResponse> LogoutAsync(int userId)
    {
        var tokens = await tasksDbContext.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();
        if (tokens.Count == 0) return new() { Success = true };
        tasksDbContext.RefreshTokens.RemoveRange(tokens);
        if (await tasksDbContext.SaveChangesAsync() >= 0) return new() { Success = true };
        return new() { Error = "Unable to logout user", ErrorCode = "L04" };
    }

    public async Task<SignupResponse> SignupAsync(SignupRequest request)
    {
        var existingUser = await tasksDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser is not null)
            return new() { Error = "User already exists with the same email", ErrorCode = "S02" };
        if (request.Password != request.ConfirmPassword)
            return new() { Error = "Password and confirm password do not match", ErrorCode = "S03" };
        if (request.Password.Length <= 7)
            return new() { Error = "Password is weak", ErrorCode = "S04" };

        var salt = PasswordHelper.GetSecureSalt();
        var user = new User
        {
            Email = request.Email, Password = PasswordHelper.HashUsingPbkdf2(request.Password, salt),
            PasswordSalt = Convert.ToBase64String(salt), FirstName = request.FirstName,
            LastName = request.LastName, Ts = DateTime.Now, IsActive = true
        };
        await tasksDbContext.Users.AddAsync(user);
        if (await tasksDbContext.SaveChangesAsync() >= 0)
            return new() { Success = true, Email = user.Email };
        return new() { Error = "Unable to save the user", ErrorCode = "S05" };
    }
}

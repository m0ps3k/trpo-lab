using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskApi.Data.Requests;
using TaskApi.Data.Responses;
using TaskApi.Interfaces;

namespace TaskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService, ITokenService tokenService) : BaseApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (request is null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest(new TokenResponse { Error = "Missing login details", ErrorCode = "L01" });
        var response = await userService.LoginAsync(request);
        if (!response.Success) return Unauthorized(new { response.ErrorCode, response.Error });
        return Ok(response);
    }

    [HttpPost("refresh_token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        if (request is null || string.IsNullOrEmpty(request.RefreshToken) || request.UserId == 0)
            return BadRequest(new TokenResponse { Error = "Missing refresh token details", ErrorCode = "R01" });
        var validation = await tokenService.ValidateRefreshTokenAsync(request);
        if (!validation.Success) return UnprocessableEntity(validation);
        var tokens = await tokenService.GenerateTokensAsync(validation.UserId);
        return Ok(new { AccessToken = tokens!.Item1, RefreshToken = tokens.Item2 });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            if (errors.Count > 0)
                return BadRequest(new TokenResponse { Error = string.Join(",", errors), ErrorCode = "S01" });
        }
        var response = await userService.SignupAsync(request);
        if (!response.Success) return UnprocessableEntity(response);
        return Ok(response.Email);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var response = await userService.LogoutAsync(UserID);
        if (!response.Success) return UnprocessableEntity(response);
        return Ok();
    }
}

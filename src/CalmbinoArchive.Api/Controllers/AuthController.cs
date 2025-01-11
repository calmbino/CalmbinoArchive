using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using CalmbinoArchive.Application.Interfaces;
using CalmbinoArchive.Application.Interfaces.Authentication;
using CalmbinoArchive.Domain.Entities.Identity;
using CalmbinoArchive.Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace CalmbinoArchive.Api.Controllers;

public record LoginResponse(string AccessToken, string RefreshToken);

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    ILogger<AuthController> logger,
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    ICacheService cache,
    ITokenService tokenService
) : ControllerBase
{
    [HttpDelete("cacheDelete")]
    public async Task<ActionResult<bool>> CacheDelete()
    {
        // await cache.RemoveAsync("user_calmbino@gmail.com");
        await cache.RemoveByPrefixAsync("user");
        return true;
    }

    [HttpPost("login", Name = "User Login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var cacheKey = $"user_{dto.Email}";
        var cachedUser = await cache.GetAsync<User>(cacheKey);

        if (cachedUser is null)
        {
            logger.LogInformation("Not logged in");
        }
        else
        {
            logger.LogInformation("Logged in");
        }

        var selectedUser = await userManager.FindByEmailAsync(dto.Email);

        await cache.SetAsync<User>(cacheKey, selectedUser, TimeSpan.FromMinutes(30));

        var loginResult = await signInManager.PasswordSignInAsync(selectedUser, dto.Password, false, false);

        logger.LogInformation("Login Result : {@loginResult}", loginResult);

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            logger.LogInformation("User Authenticated");
            var name = HttpContext.User.Identity.Name;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)
                                  .Value;
            logger.LogInformation("User name : {@name}", name);
            logger.LogInformation("User role : {@role}", role);
        }
        else
        {
            logger.LogInformation("User Not Authenticated");
        }

        var accessToken = await tokenService.GenerateAccessToken(selectedUser);
        var refreshToken = tokenService.GenerateRefreshToken();

        return new LoginResponse(accessToken, refreshToken);
    }
}

public class LoginRequestDto
{
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using CalmbinoArchive.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace CalmbinoArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    ILogger<AuthController> logger,
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IDistributedCache cache
) : ControllerBase
{
    [HttpPost("login", Name = "User Login")]
    public async Task<ActionResult<bool>> Login(LoginRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var cacheKey = $"user_{dto.Email}";
        var cachedUser = await cache.GetAsync(cacheKey);

        if (cachedUser is null)
        {
            logger.LogInformation("Not logged in");
        }
        else
        {
            logger.LogInformation("Logged in");
        }

        var selectedUser = await userManager.FindByEmailAsync(dto.Email);

        await cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(selectedUser)),
            new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) });

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

        return true;
    }
}

public class LoginRequestDto
{
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
}
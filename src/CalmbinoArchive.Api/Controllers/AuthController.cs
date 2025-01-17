using System.Security.Claims;
using AutoMapper;
using CalmbinoArchive.Application.DTOs;
using CalmbinoArchive.Application.Extensions;
using CalmbinoArchive.Application.Interfaces;
using CalmbinoArchive.Application.Interfaces.Authentication;
using CalmbinoArchive.Domain.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalmbinoArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    ILogger<AuthController> logger,
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    ICacheService cache,
    ITokenService tokenService,
    IValidator<LoginRequestDto> loginRequestDtoValidator,
    IMapper mapper
) : ControllerBase
{
    [HttpPost("login", Name = "User Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<ProblemHttpResult, Ok<LoginResponseDto>>> Login(LoginRequestDto dto)
    {
        var validationResult = await loginRequestDtoValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return TypedResults.Problem(statusCode: StatusCodes.Status400BadRequest,
                detail: "LoginRequestDto is invalid",
                extensions: [validationResult.ToErrors()]);
        }

        var cacheKey = $"{nameof(CachedUser)}:{dto.Email}";
        var cachedUser = await cache.GetAsync<CachedUser>(cacheKey);

        if (cachedUser != null)
        {
            return TypedResults.Problem(statusCode: StatusCodes.Status400BadRequest,
                detail: "Already logged in");
        }

        var selectedUser = await userManager.FindByEmailAsync(dto.Email);

        if (selectedUser == null)
        {
            return TypedResults.Problem(statusCode: StatusCodes.Status400BadRequest,
                detail: "Email or password is incorrect");
        }

        var isCorrectPassword = await userManager.CheckPasswordAsync(selectedUser, dto.Password);
        if (!isCorrectPassword)
        {
            return TypedResults.Problem(statusCode: StatusCodes.Status400BadRequest,
                detail: "Email or password is incorrect");
        }

        var accessToken = await tokenService.GenerateAccessToken(selectedUser);
        var refreshToken = tokenService.GenerateRefreshToken();

        var expirationPeriod = TimeSpan.FromDays(30);
        var expirationDate = DateTime.UtcNow.Add(expirationPeriod);

        var newCachedUser = mapper.Map<CachedUser>(selectedUser);
        newCachedUser.RefreshToken = refreshToken;
        newCachedUser.ExpirationDate = expirationDate;
        await cache.SetAsync(cacheKey, newCachedUser, expirationPeriod);

        // 쿠키 설정
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // JavaScript 접근 방지
            Secure = true, // HTTPS에서만 전송
            SameSite = SameSiteMode.Strict, // SameSite 정책 설정
            Expires = expirationDate // 쿠키 만료 시간 설정
        };
        Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);

        return TypedResults.Ok(new LoginResponseDto(accessToken));
    }

    [Authorize]
    [HttpPost("refresh-token", Name = "Refresh access token")]
    public async Task<IResult> RefreshToken()
    {
        if (!Request.Cookies.TryGetValue("RefreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
        {
            return TypedResults.Problem(statusCode: StatusCodes.Status400BadRequest,
                detail: "Refresh Token is missing or invalid.");
        }


        var email = User.FindFirst(ClaimTypes.Email)
                        ?.Value;

        var cacheKey = $"{nameof(CachedUser)}:{email}";
        var cachedUser = await cache.GetAsync<CachedUser>(cacheKey);

        if (cachedUser is null || cachedUser.RefreshToken != refreshToken)
        {
            return TypedResults.Problem(statusCode: StatusCodes.Status400BadRequest,
                detail: "Can't refresh access token");
        }

        var utcNow = DateTime.UtcNow;
        var sessionRemainingTime = cachedUser.ExpirationDate - utcNow;

        logger.LogInformation("access token expiration >> {expiration}", cachedUser.ExpirationDate);
        logger.LogInformation("datetime utc now >> {utcNow}", utcNow);
        logger.LogInformation("session remaining time >> {sessionRemainingTime}", sessionRemainingTime);
        logger.LogInformation("TimeSpan.FromDays(7) >> {time}", TimeSpan.FromDays(7));

        string? newRefreshToken = null;
        if (sessionRemainingTime <= TimeSpan.FromDays(7))
        {
            newRefreshToken = tokenService.GenerateRefreshToken();
        }

        var newAccessToken = await tokenService.GenerateAccessToken(cachedUser);

        if (newRefreshToken is not null)
        {
            var expirationPeriod = TimeSpan.FromDays(30);
            var expirationDate = DateTime.UtcNow.Add(expirationPeriod);

            cachedUser.RefreshToken = newRefreshToken;
            cachedUser.ExpirationDate = expirationDate;
            await cache.SetAsync(cacheKey, cachedUser, expirationPeriod);

            // 쿠키 설정
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // JavaScript 접근 방지
                Secure = true, // HTTPS에서만 전송
                SameSite = SameSiteMode.Strict, // SameSite 정책 설정
                Expires = expirationDate // 쿠키 만료 시간 설정
            };
            Response.Cookies.Append("RefreshToken", newRefreshToken, cookieOptions);
        }


        return TypedResults.Ok(new LoginResponseDto() { AccessToken = newAccessToken });
    }
}
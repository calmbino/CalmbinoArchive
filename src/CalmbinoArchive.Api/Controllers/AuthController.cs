using AutoMapper;
using CalmbinoArchive.Application.DTOs;
using CalmbinoArchive.Application.Extensions;
using CalmbinoArchive.Application.Interfaces;
using CalmbinoArchive.Application.Interfaces.Authentication;
using CalmbinoArchive.Domain.Entities.Identity;
using FluentValidation;
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

        var newCachedUser = mapper.Map<CachedUser>(selectedUser);
        newCachedUser.RefreshToken = refreshToken;
        await cache.SetAsync(cacheKey, newCachedUser, TimeSpan.FromDays(30));

        return TypedResults.Ok(new LoginResponseDto(accessToken, refreshToken));
    }
}
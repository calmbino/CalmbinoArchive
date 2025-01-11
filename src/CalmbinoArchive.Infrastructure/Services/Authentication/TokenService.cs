using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CalmbinoArchive.Application.Interfaces.Authentication;
using CalmbinoArchive.Domain.Contracts;
using CalmbinoArchive.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CalmbinoArchive.Infrastructure.Services.Authentication;

public class TokenService
    : ITokenService
{
    private readonly SymmetricSecurityKey _secretKey;
    private readonly string? _issuer;
    private readonly string? _audience;
    private readonly DateTime _expires;
    private readonly ILogger<TokenService> _logger;
    private readonly UserManager<User> _userManager;

    public TokenService(IConfiguration config, UserManager<User> userManager, ILogger<TokenService> logger)
    {
        _logger = logger;
        _userManager = userManager;
        var jwtSettings = config.GetSection("JwtSettings")
                                .Get<JwtSettings>();

        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.SecretKey))
        {
            throw new InvalidOperationException("JWT secret key is not configured.");
        }

        _logger.LogInformation("JWT token settings loaded >>> {@jwtSettings}", jwtSettings);

        _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
        _issuer = jwtSettings.Issuer;
        _audience = jwtSettings.Audience;
        _expires = DateTime.Now.AddDays(1);
    }

    public async Task<string> GenerateAccessToken(User user)
    {
        var signingCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
        var claims = await GetClaimsAsync(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    private async Task<List<Claim>> GetClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        var roles = await _userManager.GetRolesAsync(user!);
        claims.Add(new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty));

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials credentials, List<Claim> claims)
    {
        return new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: _expires,
            signingCredentials: credentials
        );
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CalmbinoArchive.Application.Interfaces.Authentication;
using CalmbinoArchive.Domain.Contracts;
using CalmbinoArchive.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

    public TokenService(IOptionsMonitor<JwtSettings> jwtSettings, UserManager<User> userManager,
        ILogger<TokenService> logger)
    {
        _logger = logger;
        _userManager = userManager;

        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.CurrentValue.SecretKey))
        {
            throw new InvalidOperationException("JWT secret key is not configured.");
        }

        _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.CurrentValue.SecretKey));
        _issuer = jwtSettings.CurrentValue.Issuer;
        _audience = jwtSettings.CurrentValue.Audience;
        _expires = DateTime.UtcNow.AddDays(1);
    }

    public async Task<string> GenerateAccessToken(User user)
    {
        var signingCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
        var claims = await GetClaimsAsync(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenOptions);
        return tokenHandler.WriteToken(token);
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

    private SecurityTokenDescriptor GenerateTokenOptions(SigningCredentials credentials, List<Claim> claims)
    {
        return new SecurityTokenDescriptor()
        {
            Issuer = _issuer,
            Audience = _audience,
            Subject = new ClaimsIdentity(claims),
            Expires = _expires,
            SigningCredentials = credentials
        };
    }
}
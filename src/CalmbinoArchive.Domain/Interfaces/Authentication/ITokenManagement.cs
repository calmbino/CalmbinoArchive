using System.Security.Claims;

namespace CalmbinoArchive.Domain.Interfaces.Authentication;

public interface ITokenManagement
{
    string GetRefreshToken();
    List<Claim> GetUserClaimsFromToken(string email);
    Task<bool> ValidateRefreshToken(string refreshToken);
    Task<string> GetUserIdByRefreshToken(string refreshToken);
    Task<int> AddRefreshToken(string userId, string refreshToken);
    Task<int> UpdateRefreshToken(string userId, string refreshToken);
    string GenerateToken(List<Claim> claims);
}
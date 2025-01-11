using CalmbinoArchive.Domain.Entities.Identity;

namespace CalmbinoArchive.Application.Interfaces.Authentication;

public interface ITokenService
{
    Task<string> GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
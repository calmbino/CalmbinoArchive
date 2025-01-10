using CalmbinoArchive.Domain.Entities.Identity;

namespace CalmbinoArchive.Domain.Interfaces.Authentication;

public interface IRoleManagement
{
    Task<string?> GetUserRole(string email);
    Task<bool> AddUserToRole(User user, string roleName);
}
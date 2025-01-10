using System.Security.Claims;
using CalmbinoArchive.Domain.Entities.Identity;
using CalmbinoArchive.Domain.Interfaces.Authentication;
using CalmbinoArchive.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CalmbinoArchive.Infrastructure.Repositories.Authentication;

public class RoleManagement(UserManager<User> userManager) : IRoleManagement
{
    public async Task<bool> AddUserToRole(User user, string roleName) =>
        (await userManager.AddToRoleAsync(user, roleName)).Succeeded;

    public async Task<string?> GetUserRole(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        return user == null ? null : (await userManager.GetRolesAsync(user)).FirstOrDefault();
    }
}

public class UserManagement(IRoleManagement roleManagement, UserManager<User> userManager, DataContext context)
    : IUserManagement
{
    public async Task<bool> CreateUser(User user)
    {
        var selectedUser = await GetUserByEmail(user.Email!);

        return selectedUser == null && (await userManager.CreateAsync(user, user.PasswordHash!)).Succeeded;
    }

    public async Task<User?> GetUserByEmail(string email) => await userManager.FindByEmailAsync(email);

    public async Task<User?> GetUserById(string id) => await userManager.FindByIdAsync(id);

    public async Task<IEnumerable<User>?> GetAllUsers() => await context.Users.ToListAsync();

    public async Task<List<Claim>> GetUserClaims(string email)
    {
        var selectedUser = await GetUserByEmail(email);
        var roleName = await roleManagement.GetUserRole(selectedUser!.Email!);

        List<Claim> claims =
        [
            new Claim("UserName", selectedUser!.UserName!),
            new Claim(ClaimTypes.NameIdentifier, selectedUser!.Id!),
            new Claim(ClaimTypes.Email, selectedUser!.Email!),
            new Claim(ClaimTypes.Role, roleName!)
        ];

        return claims;
    }

    public async Task<bool> LoginUser(User user)
    {
        var selectedUser = await GetUserByEmail(user.Email!);
        if (selectedUser is null) return false;

        var roleName = await roleManagement.GetUserRole(selectedUser.Email!);
        if (string.IsNullOrEmpty(roleName)) return false;

        return await userManager.CheckPasswordAsync(selectedUser, user.PasswordHash!);
    }

    public async Task<int> RemoveUserByEmail(string email)
    {
        var selectedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        context.Users.Remove(selectedUser!);
        return await context.SaveChangesAsync();
    }
}

public class TokenManagement(DataContext context, IConfiguration config) : ITokenManagement
{
    public string GetRefreshToken()
    {
        throw new NotImplementedException();
    }

    public List<Claim> GetUserClaimsFromToken(string email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateRefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserIdByRefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> AddRefreshToken(string userId, string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateRefreshToken(string userId, string refreshToken)
    {
        throw new NotImplementedException();
    }

    public string GenerateToken(List<Claim> claims)
    {
        throw new NotImplementedException();
    }
}
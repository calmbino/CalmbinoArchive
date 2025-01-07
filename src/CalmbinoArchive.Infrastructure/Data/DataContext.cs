using CalmbinoArchive.Domain.Entities.Identity;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CalmbinoArchive.Infrastructure.Data;

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 테이블 이름 변경
        builder.Entity<User>(entity =>
        {
            entity.ToTable("User"); // 기본 테이블 AspNetUsers → Users로 변경
        });

        builder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable("Role"); // 기본 테이블 AspNetRoles → Roles로 변경
        });

        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRole"); // 기본 테이블 AspNetUserRoles → UserRoles로 변경
        });

        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaim"); // 기본 테이블 AspNetUserClaims → UserClaims로 변경
        });

        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaim"); // 기본 테이블 AspNetRoleClaims → RoleClaims로 변경
        });

        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogin"); // 기본 테이블 AspNetUserLogins → UserLogins로 변경
        });

        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserToken"); // 기본 테이블 AspNetUserTokens → UserTokens로 변경
        });
    }
}
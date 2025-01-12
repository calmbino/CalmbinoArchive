using System.Diagnostics;
using CalmbinoArchive.Domain.Entities.Identity;
using CalmbinoArchive.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;

namespace CalmbinoArchive.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();


            await EnsureDatabaseAsync(dbContext, cancellationToken);
            await RunMigrationAsync(dbContext, cancellationToken);
            await SeedDataAsync(dbContext, serviceProvider, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(DataContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task RunMigrationAsync(DataContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // ExecutionStrategy가 전적으로 트랜잭션을 관리하기 때문에 사용자 정의 트랜잭션이 존재하면 충돟한다.
            // 또한 MigrateAsync는 각 마이그레이션 단계마다 별도의 트랜잭션을 자동으로 생성
            // 그러므로 사용자가 직접 정의한 트랜잭션은 일단 주석 처리

            // Run migration in a transaction to avoid partial migration if it fails.
            // await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            // await transaction.CommitAsync(cancellationToken);
        });

        // 또다른 표현 방법
        // await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);
    }

    private static async Task SeedDataAsync(DataContext dbContext, IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new List<IdentityRole>
        {
            new() { Name = "Member" },
            new() { Name = "Admin" },
        };

        User admin = new()
        {
            FirstName = "Gildong",
            LastName = "Hong",
            UserName = "Calmbino",
            Email = "calmbino@gmail.com",
        };

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database(User & Role)
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var result = await userManager.CreateAsync(admin, "Test!@#123");
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Code} - {error.Description}");
                }
            }

            await userManager.AddToRoleAsync(admin, "Admin");

            await dbContext.SaveChangesAsync(cancellationToken);
        });
    }
}
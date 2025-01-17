using CalmbinoArchive.Domain.Entities.Identity;
using CalmbinoArchive.Infrastructure.Data;
using CalmbinoArchive.Infrastructure.Extensions;
using CalmbinoArchive.MigrationService;
using CalmbinoArchive.ServiceDefaults;
using Microsoft.AspNetCore.Identity;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
       .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

// Seeding에서 UserManager & RoleManager를 사용하기 위해 Identity 추가
builder.Services.AddIdentity<User, IdentityRole>()
       .AddEntityFrameworkStores<DataContext>();

builder.AddPostgreSql();

var host = builder.Build();
host.Run();
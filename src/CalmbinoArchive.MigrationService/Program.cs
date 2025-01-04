using CalmbinoArchive.Infrastructure.Data;
using CalmbinoArchive.MigrationService;
using CalmbinoArchive.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
       .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<DataContext>("CalmbinoArchive");

var host = builder.Build();
host.Run();
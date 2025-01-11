using CalmbinoArchive.Application.Extensions;
using CalmbinoArchive.Application.Interfaces;
using CalmbinoArchive.Application.Interfaces.Authentication;
using CalmbinoArchive.Domain.Entities.Identity;
using CalmbinoArchive.Infrastructure.Extensions;
using CalmbinoArchive.Infrastructure.Middlewares;
using CalmbinoArchive.Infrastructure.Services;
using CalmbinoArchive.Infrastructure.Services.Authentication;
using CalmbinoArchive.ServiceDefaults;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// TODO: move to infrastructure Layer
Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
                                      .WriteTo.Console()
                                      .WriteTo.File("../../logs/api_.txt", rollingInterval: RollingInterval.Day)
                                      .CreateLogger();
// builder.Host.UseSerilog((context, configuration) =>
// {
//     configuration.WriteTo.Console();
//     configuration.WriteTo.File("../../logs/api_.txt", rollingInterval: RollingInterval.Day);
// });
builder.Services.AddSerilog(Log.Logger);

Log.Information("Application is building.....");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://localhost:7036",
                      "http://localhost:5196")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});


// Add aspire Services
builder.AddServiceDefaults();

// Add PostgreSQL
builder.AddPostgreDatabase();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplication()
       .AddInfrastructure(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICacheService, CacheService>();


try
{
    var app = builder.Build();
    app.UseSerilogRequestLogging();

    // Configure aspire
    app.MapDefaultEndpoints();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options => { options.Theme = ScalarTheme.Moon; });
        app.UseDeveloperExceptionPage();
    }

    // app.UseExceptionHandler("/Error");
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseExceptionHandler();

    app.MapControllers();

    app.UseCors();

    app.UseAuthentication();
    app.UseAuthorization();

    Log.Information("Application is running.....");
    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex, "Application failed to start.....");
}
finally
{
    Log.CloseAndFlush();
}
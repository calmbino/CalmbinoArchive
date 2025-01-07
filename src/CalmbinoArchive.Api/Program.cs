using CalmbinoArchive.Application.Extensions;
using CalmbinoArchive.Infrastructure.Extensions;
using CalmbinoArchive.Infrastructure.Middlewares;
using CalmbinoArchive.ServiceDefaults;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
                                      .WriteTo.Console()
                                      .WriteTo.File("../../logs/api.log.txt", rollingInterval: RollingInterval.Day)
                                      .CreateLogger();

builder.Host.UseSerilog();

Log.Logger.Information("Application is building.....");

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
        app.MapScalarApiReference();
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

    Log.Logger.Information("Application is running.....");
    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Error(ex, "Application failed to start.....");
}
finally
{
    Log.CloseAndFlush();
}
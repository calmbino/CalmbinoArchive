using CalmbinoArchive.Application.Extensions;
using CalmbinoArchive.Application.Interfaces;
using CalmbinoArchive.Application.Interfaces.Authentication;
using CalmbinoArchive.Domain.Contracts;
using CalmbinoArchive.Infrastructure.Extensions;
using CalmbinoArchive.Infrastructure.Services;
using CalmbinoArchive.Infrastructure.Services.Authentication;
using CalmbinoArchive.ServiceDefaults;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

// TODO: move to infrastructure Layer
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                                      // .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                                      .Enrich.FromLogContext()
                                      .WriteTo.Console()
                                      .WriteTo.File("../../logs/api_.txt", rollingInterval: RollingInterval.Day)
                                      .CreateLogger();
// builder.Host.UseSerilog((context, configuration) =>
// {
//     configuration.WriteTo.Console();
//     configuration.WriteTo.File("../../logs/api_.txt", rollingInterval: RollingInterval.Day);
// });
builder.Services.AddSerilog(Log.Logger);

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        Log.Information("CustomizeProblemDetails 진입!!!!!!!!!!!!!!!!!!");
    };
});

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


builder.Services
       .AddScoped<ITokenService,
           TokenService>(); // TODO: TokenService를 sigleton으로 사용하고 싶다면, UserManager에 대한 의존성을 없애야 한다.
builder.Services.AddScoped<ICacheService, CacheService>();


try
{
    var app = builder.Build();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseExceptionHandler();

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

    app.UseHttpsRedirection();


    app.MapControllers();

    app.UseCors();


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
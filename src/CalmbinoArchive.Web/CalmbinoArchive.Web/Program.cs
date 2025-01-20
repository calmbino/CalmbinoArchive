using CalmbinoArchive.Application.Extensions;
using CalmbinoArchive.Infrastructure.Extensions;
using CalmbinoArchive.ServiceDefaults;
using CalmbinoArchive.Web.Client.Pages;
using CalmbinoArchive.Web.Components;
using CalmbinoArchive.Web.Shared.Extensions;
using Microsoft.AspNetCore.CookiePolicy;
using MudBlazor.Services;
using Serilog;
using Solutaris.InfoWARE.ProtectedBrowserStorage.Extensions;

var builder = WebApplication.CreateBuilder(args);

var baseDirectory = AppContext.BaseDirectory;
var projectRoot = baseDirectory.Split("src")[0];
var logsPath = Path.Combine(projectRoot, "logs");

// TODO: move to infrastructure Layer
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                                      // .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                                      .Enrich.FromLogContext()
                                      .WriteTo.Console()
                                      .WriteTo.File($"{logsPath}/blazor_.txt", rollingInterval: RollingInterval.Day)
                                      .CreateLogger();

builder.Services.AddSerilog(Log.Logger);

// Add aspire services
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents()
       .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpContextAccessor();

builder.Services.AddWebShared(builder.Configuration)
       .AddApplication();

builder.Services.AddIWProtectedBrowserStorage();

var app = builder.Build();

// Configure aspire
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode()
   .AddInteractiveWebAssemblyRenderMode()
   .AddAdditionalAssemblies(typeof(CalmbinoArchive.Web.Client._Imports).Assembly);

app.Run();
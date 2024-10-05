using CalmbinoArchive.Application;
using CalmbinoArchive.Infrastructure;
using CalmbinoArchive.Web.Components;
using CalmbinoArchive.Web.Shared;
using MudBlazor.Services;
using _Imports = CalmbinoArchive.Web.Client._Imports;

var builder = WebApplication.CreateBuilder(args);

// Add application layer
builder.Services.AddApplication();
// Add infrastructure layer
builder.Services.AddInfrastructure();
// Add Shared for Blazor
builder.Services.AddWebShared();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents()
       .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode()
   .AddInteractiveWebAssemblyRenderMode()
   .AddAdditionalAssemblies(typeof(_Imports).Assembly);

app.Run();
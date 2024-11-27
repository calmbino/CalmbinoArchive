using CalmbinoArchive.Application.Extensions;
using CalmbinoArchive.Infrastructure.Extensions;
using CalmbinoArchive.Web.Client.Pages;
using CalmbinoArchive.Web.Components;
using CalmbinoArchive.Web.Shared.Extensions;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add aspire services
builder.AddServiceDefaults();


// Add services to the container.
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents()
       .AddInteractiveWebAssemblyComponents();
builder.Services.AddWebShared()
       .AddApplication()
       .AddInfrastructure(builder.Configuration);


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
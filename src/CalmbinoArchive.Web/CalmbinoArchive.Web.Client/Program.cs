using CalmbinoArchive.Application;
using CalmbinoArchive.Web.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add application layer
builder.Services.AddApplication();

// Add Shared for Blazor
builder.Services.AddWebShared();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

await builder.Build().RunAsync();
using CalmbinoArchive.Application;
using CalmbinoArchive.Infrastructure;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add application layer
builder.Services.AddApplication();
// Add infrastructure layer
builder.Services.AddInfrastructure();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

await builder.Build().RunAsync();
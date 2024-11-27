using CalmbinoArchive.Web.Shared.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add MudBlazor Services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddWebShared();

await builder.Build()
             .RunAsync();
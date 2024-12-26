using System.Net;
using System.Text.Json;
using CalmbinoArchive.Web.Shared.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ServiceDiscovery;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// client(webassembly)에서 service 이름으로 API 호출하기 위한 설정
// wwwroot/appsettings.json로부터 탐색
builder.Services.AddServiceDiscovery();
builder.Services.ConfigureHttpClientDefaults(http => { http.AddServiceDiscovery(); });

// Add services to the container.
builder.Services.AddWebShared(builder.Configuration);

await builder.Build()
             .RunAsync();
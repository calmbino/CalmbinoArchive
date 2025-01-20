using CalmbinoArchive.Web.Shared.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Debug()
             .WriteTo.BrowserConsole()
             .CreateLogger();

// client(webassembly)에서 service 이름으로 API 호출하기 위한 설정
// wwwroot/appsettings.json로부터 탐색
builder.Services.AddServiceDiscovery();
builder.Services.ConfigureHttpClientDefaults(http => { http.AddServiceDiscovery(); });

// Add services to the container.
builder.Services.AddWebShared(builder.Configuration);

await builder.Build()
             .RunAsync();
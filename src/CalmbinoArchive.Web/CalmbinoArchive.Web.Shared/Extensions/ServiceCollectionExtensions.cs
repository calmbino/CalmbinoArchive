using System.Net;
using CalmbinoArchive.Web.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;

namespace CalmbinoArchive.Web.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebShared(this IServiceCollection services, IConfiguration configuration)
    {
        // Add MudBlazor Services
        services.AddMudServices();

        // TODO: Find ways to leverage Aspire's Service Discovery
        // [임시 해결책 - 241226]  
        // - blazor server에서는 문제없음
        // - blazor client(webassembly)에서는 별도로 service discovery를 설정해줘야 함
        services.AddScoped<WeatherClientService>();
        services.AddScoped<AuthClientService>();
        services.AddHttpClient<WeatherClientService>(static client => client.BaseAddress = new("https+http://backend"));
        services.AddHttpClient<AuthClientService>(static client => client.BaseAddress = new("https+http://backend"));


        return services;
    }
}
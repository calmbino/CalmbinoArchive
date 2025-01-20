using CalmbinoArchive.Web.Shared.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        services.AddTransient<CookieHandler>();
        SetCalmbinoArchiveHttpClient<WeatherClientService>(services);
        SetCalmbinoArchiveHttpClient<AuthClientService>(services);

        return services;
    }

    /**
     * Blazor에서 사용하는 HttpClient 서비스에 CalmbinoArchive API 사용하도록 설정
     */
    private static void SetCalmbinoArchiveHttpClient<T>(this IServiceCollection services) where T : class
    {
        services.AddHttpClient<T>(static client => { client.BaseAddress = new("https+http://backend"); })
                .AddHttpMessageHandler<CookieHandler>(); // <- Blazor Client(wasm)에서 필요한 위한 설정
    }
}

internal class CookieHandler : DelegatingHandler
{
    /**
     * 해당 설정을 포함해야 Browser가 서버로부터 받은 쿠키를 자동으로 저장하고, 요청을 보낼 때 헤더에 쿠키를 자동으로 포함
     */
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        request.Headers.Add("X-Requested-With", ["XMLHttpRequest"]);

        return base.SendAsync(request, cancellationToken);
    }
}
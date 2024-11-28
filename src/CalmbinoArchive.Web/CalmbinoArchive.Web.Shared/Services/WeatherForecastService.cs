using System.Net.Http.Json;
using CalmbinoArchive.Domain.Entities;

namespace CalmbinoArchive.Web.Shared.Services;

public class WeatherForecastService
{
    private readonly HttpClient _httpClient;

    WeatherForecastService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CalmbinoAPI");
    }

    public async Task<WeatherForecast[]> GetForecastAsync()
    {
        return await _httpClient.GetFromJsonAsync<WeatherForecast[]>("/api/WeatherForecast") ?? [];
    }
}
using System.Net.Http.Json;
using System.Text.Json;
using CalmbinoArchive.Domain.Entities;

namespace CalmbinoArchive.Web.Shared.Services;

public class WeatherForecastService
{
    private readonly HttpClient _httpClient;

    public WeatherForecastService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CalmbinoArchive-Api");
    }

    public async Task<WeatherForecast[]> GetDatasAsync()
    {
        return await _httpClient.GetFromJsonAsync<WeatherForecast[]>("/api/WeatherForecast") ?? [];
    }
}
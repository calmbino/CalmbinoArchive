using System.Net.Http.Json;
using CalmbinoArchive.Domain.Entities;

namespace CalmbinoArchive.Web.Shared.Services;

public class WeatherForecastService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("CalmbinoArchiveAPI");


    public async Task<WeatherForecast[]> GetDataAsync()
    {
        return await _httpClient.GetFromJsonAsync<WeatherForecast[]>("/WeatherForecast") ?? [];
    }
}
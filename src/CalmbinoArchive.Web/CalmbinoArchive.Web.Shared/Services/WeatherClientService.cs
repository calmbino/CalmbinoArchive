using System.Net.Http.Json;
using System.Text.Json;
using CalmbinoArchive.Domain.Entities;

namespace CalmbinoArchive.Web.Shared.Services;

public class WeatherClientService(HttpClient httpClient)
{
    public async Task<WeatherForecast[]> GetDatasAsync()
    {
        return await httpClient.GetFromJsonAsync<WeatherForecast[]>("/api/WeatherForecast") ?? [];
    }
}
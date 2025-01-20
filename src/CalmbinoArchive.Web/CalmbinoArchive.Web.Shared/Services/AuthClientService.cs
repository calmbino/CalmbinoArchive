using System.Net.Http.Json;
using CalmbinoArchive.Application.DTOs;

namespace CalmbinoArchive.Web.Shared.Services;

public class AuthClientService(HttpClient httpClient)
{
    public Task<HttpResponseMessage> LoginAsync(LoginRequestDto dto)
    {
        return httpClient.PostAsJsonAsync("/api/auth/login", dto);
    }
}
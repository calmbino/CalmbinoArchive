using System.Net.Http.Json;
using CalmbinoArchive.Application.DTOs;

namespace CalmbinoArchive.Web.Shared.Services;

public class AuthClientService(HttpClient httpClient)
{
    public async Task<HttpResponseMessage> LoginAsync(LoginRequestDto dto)
    {
        return await httpClient.PostAsJsonAsync<LoginRequestDto>("/api/auth/login", dto);
    }
}
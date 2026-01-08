using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using At.luki0606.DartZone.Shared.Dtos.Requests;

namespace At.luki0606.DartZone.AvaloniaUI.Services.AuthService;

internal sealed class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<bool> LoginAsync(UserRequestDto dto)
    {
        string json = JsonSerializer.Serialize(dto, _serializerOptions);
        using StringContent content = new(json, System.Text.Encoding.UTF8, "application/json");
        Uri route = new("auth/login", UriKind.Relative);
        HttpResponseMessage response = await _httpClient.PostAsync(route, content).ConfigureAwait(false);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RegisterAsync(UserRequestDto dto)
    {
        string json = JsonSerializer.Serialize(dto, _serializerOptions);
        using StringContent content = new(json, System.Text.Encoding.UTF8, "application/json");
        Uri route = new("auth/register", UriKind.Relative);
        HttpResponseMessage response = await _httpClient.PostAsync(route, content).ConfigureAwait(false);
        return response.IsSuccessStatusCode;
    }
}

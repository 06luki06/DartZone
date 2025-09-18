using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using At.luki0606.DartZone.Shared.Dtos.Requests;

namespace At.luki0606.DartZone.AvaloniaUI.Services.AuthService
{
    public class AuthService : IAuthService
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
            HttpResponseMessage response = await _httpClient.PostAsync("auth/login", content);
            Debug.WriteLine(response.StatusCode);
            Debug.WriteLine(await response.Content.ReadAsStringAsync());
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RegisterAsync(UserRequestDto dto)
        {
            string json = JsonSerializer.Serialize(dto, _serializerOptions);
            using StringContent content = new(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("auth/register", content);
            Debug.WriteLine(response.StatusCode);
            Debug.WriteLine(await response.Content.ReadAsStringAsync());
            return response.IsSuccessStatusCode;
        }
    }
}

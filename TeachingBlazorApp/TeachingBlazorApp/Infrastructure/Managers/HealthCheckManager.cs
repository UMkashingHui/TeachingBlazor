using System.Text.Json;

namespace TeachingBlazorApp.Infrastructure.Managers
{
    public class HealthCheckManager
    {
        private readonly HttpClient _httpClient;

        public HealthCheckManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetDataAsync()
        {
            var response = await _httpClient.GetAsync("api/healthcheck");
            var responseAsString = await response.Content.ReadAsStringAsync();
            return responseAsString;
        }
    }
}
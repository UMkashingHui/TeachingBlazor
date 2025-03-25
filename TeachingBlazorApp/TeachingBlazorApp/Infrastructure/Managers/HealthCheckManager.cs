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
            try{
                var response = await _httpClient.GetAsync("api/healthcheck");
                var responseAsString = await response.Content.ReadAsStringAsync();
                return responseAsString;
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new { Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SafeMum.Application.Interfaces;


namespace SafeMum.Infrastructure.Services
{
    namespace SafeMum.Infrastructure.Services
    {
        public class NodePushNotificationService : IPushNotificationService
        {
            private readonly HttpClient _httpClient;
            private readonly IConfiguration _config;

            public NodePushNotificationService(IConfiguration config)
            {
                _config = config;
                _httpClient = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(10)
                };
               
            }

            public async Task SendPushNotification(string deviceToken, string title, string body, object data = null)
            {
                var payload = new { deviceToken, title, body, data };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var url = $"{_config["NodeApiBaseUrl"].TrimEnd('/')}/api/send-push";

                var response = await _httpClient.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Push failed: {result}");
            }

            // Optional: batch
            public async Task SendPushNotifications(IEnumerable<string> deviceTokens, string title, string body, object data = null)
            {
                var payload = new { deviceTokens, title, body, data };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var url = $"{_config["NodeApiBaseUrl"].TrimEnd('/')}/api/send-push";

                var response = await _httpClient.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Push failed: {result}");
            }
        }
    }
}

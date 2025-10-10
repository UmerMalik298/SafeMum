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
    public class NodePushNotificationService : IPushNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public NodePushNotificationService(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _config = config;
        }

        public async Task SendPushNotification(string deviceToken, string title, string body, object data = null)
        {
            var payload = new
            {
                deviceToken,
                title,
                body,
                data
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_config["NodeApiBaseUrl"]}/api/send-push", content);

            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                throw new Exception($"Push failed: {result}");
            }
        }
    }
}

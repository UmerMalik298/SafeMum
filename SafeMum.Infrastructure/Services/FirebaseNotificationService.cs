using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using SafeMum.Application.Interfaces;

namespace SafeMum.Infrastructure.Services
{
    public class NodeNotificationService : IPushNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _nodeApiBaseUrl;

        public NodeNotificationService(IConfiguration config, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _nodeApiBaseUrl = config["http://localhost:5001"]; // e.g. http://localhost:5001
        }

        public async Task SendPushNotification(string deviceToken, string title, string body)
        {
            var payload = new
            {
                deviceToken,
                title,
                body
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"{_nodeApiBaseUrl}/send-notification", payload);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Node API error: {response.StatusCode} - {error}");
            }
        }
    }
}
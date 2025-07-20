using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SafeMum.Application.Interfaces;

namespace SafeMum.Infrastructure.Services
{
    public class FirebaseNotificationService : IPushNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _serverKey;

        public FirebaseNotificationService(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _serverKey = config["Firebase:ServerKey"];
        }

        public async Task SendPushNotification(string deviceToken, string title, string body)
        {
            var payload = new
            {
                to = deviceToken,
                notification = new
                {
                    title = title,
                    body = body
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
            request.Headers.TryAddWithoutValidation("Authorization", $"key={_serverKey}");
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }

}

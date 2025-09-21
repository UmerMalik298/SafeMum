using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using SafeMum.Application.Interfaces;

namespace SafeMum.Infrastructure.Services
{
    public class FirebaseNotificationService : IPushNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleCredential _googleCredential;
        private readonly string _projectId;

        public FirebaseNotificationService(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _projectId = config["Firebase:ProjectId"];

            // Load service account credentials from file
            _googleCredential = GoogleCredential
                .FromFile(config["Firebase:ServiceAccountFile"])
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");
        }

        public async Task SendPushNotification(string deviceToken, string title, string body)
        {
            // Get OAuth2 access token
            var accessToken = await _googleCredential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://fcm.googleapis.com/v1/projects/{_projectId}/messages:send");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var payload = new
            {
                message = new
                {
                    token = deviceToken,
                    notification = new
                    {
                        title,
                        body
                    }
                }
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"FCM error: {response.StatusCode} - {result}");
            }
        }
    }

}

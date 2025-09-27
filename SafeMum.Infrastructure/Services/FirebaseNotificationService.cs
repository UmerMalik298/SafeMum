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
            try
            {
                Console.WriteLine($"Attempting to send notification to token: {deviceToken}");

                // Get OAuth2 access token
                var accessToken = await _googleCredential.UnderlyingCredential.GetAccessTokenForRequestAsync();
                Console.WriteLine($"Access token obtained: {!string.IsNullOrEmpty(accessToken)}");

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

                var jsonPayload = JsonSerializer.Serialize(payload);
                Console.WriteLine($"Payload: {jsonPayload}");

                request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"FCM Response: {response.StatusCode} - {result}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"FCM error: {response.StatusCode} - {result}");
                }

                Console.WriteLine("Notification sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendPushNotification: {ex.Message}");
                throw;
            }
        }
    }
    }


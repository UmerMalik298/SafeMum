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
    public class FirebaseNotificationService : IPushNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleCredential _googleCredential;
        private readonly string _projectId;
        private readonly ILogger<FirebaseNotificationService> _logger;
        private string _cachedAccessToken;
        private DateTime _tokenExpiry;

        public FirebaseNotificationService(IConfiguration config, ILogger<FirebaseNotificationService> logger)
        {
            _httpClient = new HttpClient();
            _projectId = config["Firebase:ProjectId"];
            _logger = logger;

            _googleCredential = GoogleCredential
                .FromFile(config["Firebase:ServiceAccountFile"])
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");
        }

        public async Task SendPushNotification(string deviceToken, string title, string body, object data = null)
        {
            try
            {
                _logger.LogInformation($"Sending notification to token ending in ...{deviceToken.Substring(deviceToken.Length - 8)}");

                var accessToken = await GetAccessTokenAsync();

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"https://fcm.googleapis.com/v1/projects/{_projectId}/messages:send");

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var message = new
                {
                    message = new
                    {
                        token = deviceToken,
                        notification = new { title, body },
                        data =  data != null ? JsonSerializer.Serialize(data) : new Dictionary<string, string>(),
                        android = new
                        {
                            priority = "high",
                            notification = new
                            {   
                                sound = "default",
                                channel_id = "appointments"
                            }
                        },
                        apns = new
                        {
                            payload = new
                            {
                                aps = new
                                {
                                    alert = new { title, body },
                                    sound = "default",
                                    badge = 1
                                }
                            }
                        }
                    }
                };

                var jsonPayload = JsonSerializer.Serialize(message, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"FCM error: {response.StatusCode} - {result}");
                    throw new Exception($"FCM error: {response.StatusCode} - {result}");
                }

                _logger.LogInformation("Notification sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SendPushNotification: {ex.Message}");
                throw;
            }
        }

        private async Task<string> GetAccessTokenAsync()
        {
            if (_cachedAccessToken != null && DateTime.UtcNow < _tokenExpiry)
            {
                return _cachedAccessToken;
            }

            var accessToken = await _googleCredential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            _cachedAccessToken = accessToken;
            _tokenExpiry = DateTime.UtcNow.AddMinutes(50); // Google tokens expire in 1 hour, refresh at 50 min

            return accessToken;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
    }


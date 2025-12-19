using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SafeMum.Application.Interfaces;

namespace SafeMum.Application.Services
{
    public class SupabaseAdminService : ISupabaseAdminService
    {
        private readonly HttpClient _httpClient;
        private readonly string _supabaseUrl;
        private readonly string _serviceRoleKey;

        public SupabaseAdminService(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _supabaseUrl = config["Supabase:Url"];
            _serviceRoleKey = config["Supabase:ServiceRoleKey"];
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_supabaseUrl}/auth/v1/admin/users/{userId}")
            };

            request.Headers.Add("Authorization", $"Bearer {_serviceRoleKey}");

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }


            public async Task<SupabaseUser> GetUserByEmailAsync(string email)
            {
                try
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"{_supabaseUrl}/auth/v1/admin/users")
                    };

                    request.Headers.Add("Authorization", $"Bearer {_serviceRoleKey}");
                    request.Headers.Add("apikey", _serviceRoleKey);

                    var response = await _httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    var usersResponse = JsonSerializer.Deserialize<UsersListResponse>(content);

                    var user = usersResponse?.Users?.FirstOrDefault(u =>
                        u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                    return user != null ? new SupabaseUser
                    {
                        Id = user.Id,
                        Email = user.Email
                    } : null;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to get user by email: {ex.Message}");
                }
            }

            public async Task<bool> UpdateUserPasswordAsync(string userId, string newPassword)
            {
                try
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Put,
                        RequestUri = new Uri($"{_supabaseUrl}/auth/v1/admin/users/{userId}"),
                        Content = new StringContent(
                            JsonSerializer.Serialize(new { password = newPassword }),
                            Encoding.UTF8,
                            "application/json")
                    };

                    request.Headers.Add("Authorization", $"Bearer {_serviceRoleKey}");
                    request.Headers.Add("apikey", _serviceRoleKey);

                    var response = await _httpClient.SendAsync(request);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to update password: {ex.Message}");
                }
            }

         

            // Helper classes for deserialization
            private class UsersListResponse
            {
                [JsonPropertyName("users")]
                public List<UserData> Users { get; set; }
            }

            private class UserData
            {
                [JsonPropertyName("id")]
                public string Id { get; set; }

                [JsonPropertyName("email")]
                public string Email { get; set; }
            }
        }
    }



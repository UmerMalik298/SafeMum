using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }

}

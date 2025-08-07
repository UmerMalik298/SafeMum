using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SafeMum.Application.Interfaces;

using Supabase;
using Supabase.Gotrue;

namespace SafeMum.Infrastructure.Services
{
    public class SupabaseClientFactory : ISupabaseClientFactory
    {
        private readonly Supabase.Client _client;

        public SupabaseClientFactory(IConfiguration config)
        {
            var supabaseUrl = config["Supabase:Url"];
            var supabaseKey = config["Supabase:Key"];
            var serviceRoleKey = config["Supabase:ServiceRoleKey"];
            if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
                throw new ArgumentException("Supabase URL or Key is not configured properly");

            var options = new Supabase.SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = false
            };

            _client = new Supabase.Client(supabaseUrl, serviceRoleKey, options);
            // _client = new Supabase.Client(supabaseUrl, supabaseKey);
            _client.InitializeAsync().Wait();
        }

        public Supabase.Client GetClient() => _client;
    
}
}

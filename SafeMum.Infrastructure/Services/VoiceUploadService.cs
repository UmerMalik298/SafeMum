using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SafeMum.Application.Interfaces;
using Supabase;

namespace SafeMum.Infrastructure.Services
{
    public class VoiceUploadService : IVoiceUploadService
    {

        private readonly Supabase.Client _client;
        private readonly String _bucketName;
        public VoiceUploadService(IConfiguration configuration)
        {
            var url = configuration["Supabase:Url"];
            var key = configuration["Supabase:Key"];
            _bucketName = configuration["Supabase:Bucket"];

            _client = new Client(url, key, new SupabaseOptions
            {
                AutoConnectRealtime = false
            });

            _client.InitializeAsync().Wait();
        }

        public async Task<string?> UploadVoiceAsync(IFormFile voice)
        {
            if (voice == null || voice.Length == 0)
                return null;

            var fileName = $"{Guid.NewGuid()}_{voice.FileName}";

            using var memoryStream = new MemoryStream();
            await voice.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            try
            {
                var result = await _client.Storage
                    .From(_bucketName)
                    .Upload(fileBytes, fileName, new Supabase.Storage.FileOptions
                    {
                        CacheControl = "3600",
                        Upsert = true,
                        ContentType = voice.ContentType
                    });

                if (result == null)
                    return null;

                var publicUrl = _client.Storage
                    .From(_bucketName)
                    .GetPublicUrl(fileName);

                return publicUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Voice upload failed: {ex.Message}");
                return null;
            }
        }
    }
}

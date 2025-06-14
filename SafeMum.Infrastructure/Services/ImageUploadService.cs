using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SafeMum.Application.Interfaces;
using Supabase;

namespace SafeMum.Infrastructure.Services
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly Supabase.Client _client;
        private readonly String _bucketName;
        public ImageUploadService(IConfiguration configuration)
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
        public async Task<string?> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            // Add error handling
            try
            {
                var result = await _client.Storage
                    .From(_bucketName)
                    .Upload(fileBytes, fileName, new Supabase.Storage.FileOptions
                    {
                        CacheControl = "3600",
                        Upsert = true,
                        ContentType = file.ContentType
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
               
                Console.WriteLine($"Upload failed: {ex.Message}");
                return null;
            }
        }




    }
}

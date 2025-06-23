using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SafeMum.Application.Interfaces;

namespace SafeMum.Infrastructure.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly HttpClient _httpClient;
        public TranslationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }
        public async Task<string> TranslateToUrduAsync(string text)
        {
            var url = $"https://api.mymemory.translated.net/get?q={Uri.EscapeDataString(text)}&langpair=en|ur";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(json);
            return doc.RootElement
                      .GetProperty("responseData")
                      .GetProperty("translatedText")
                      .GetString() ?? string.Empty;
        }
    }
}

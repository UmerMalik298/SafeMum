using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;
using Supabase.Postgrest;  

using static Supabase.Postgrest.Constants;
namespace SafeMum.Application.Features.Content.GetAllContentItem
{
    public class GetAllContentItemHandler : IRequestHandler<GetAllContentItemRequest, List<GetAllContentItemResponse>>
    {
        private readonly Supabase.Client _client;

        public GetAllContentItemHandler(ISupabaseClientFactory supabaseClient)
        {
            _client = supabaseClient.GetClient();
        }
        public async Task<List<GetAllContentItemResponse>> Handle( GetAllContentItemRequest request, CancellationToken cancellationToken)
        {
            await _client.InitializeAsync();

           
            var query = _client.From<contentitem>();

            if (!string.IsNullOrEmpty(request.Category))
            {
                query = (Supabase.Interfaces.ISupabaseTable<contentitem, Supabase.Realtime.RealtimeChannel>)query.Filter(nameof(contentitem.category), Operator.Equals, request.Category);
            }

            if (!string.IsNullOrEmpty(request.Audience))
            {
                query = (Supabase.Interfaces.ISupabaseTable<contentitem, Supabase.Realtime.RealtimeChannel>)query.Filter(nameof(contentitem.audience), Operator.Equals, request.Audience);
            }

           
            var response = await query.Get();
            var items = response.Models;

          
            var language = (request.Language?.ToLower() ?? "en") switch
            {
                "ur" => "ur",
                _ => "en"
            };

            // Map response with comprehensive null handling
            var result = items.Select(item => new GetAllContentItemResponse
            {
                Id = item.Id.ToString(),
                Title = language == "ur" ? item.title_ur ?? item.title_en ?? string.Empty : item.title_en ?? string.Empty,
                Text = language == "ur" ? item.text_ur ?? item.text_en ?? string.Empty : item.text_en ?? string.Empty,
                Summary = language == "ur" ? item.summary_ur ?? item.summary_en ?? string.Empty : item.summary_en ?? string.Empty,
                ImageUrl = item.image_url ?? string.Empty,
                Category = item.category ?? string.Empty,
                Audience = item.audience ?? string.Empty,
                Tags = item.tags ?? new List<string>()
            }).ToList();

            return result;
        }
    }
}
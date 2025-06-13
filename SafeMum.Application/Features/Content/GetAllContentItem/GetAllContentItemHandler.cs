using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;

namespace SafeMum.Application.Features.Content.GetAllContentItem
{
    public class GetAllContentItemHandler : IRequestHandler<GetAllContentItemRequest,List<GetAllContentItemResponse>>
    {

        private readonly Supabase.Client _client;

        public GetAllContentItemHandler(ISupabaseClientFactory supabaseClient)
        {
            _client = supabaseClient.GetClient();

        }

        public async Task<List<GetAllContentItemResponse>> Handle(GetAllContentItemRequest request, CancellationToken cancellationToken)
        {
            await _client.InitializeAsync();

            var response = await _client.From<contentitem>().Get();
            var items = response.Models;

            var result = items.Select(item => new GetAllContentItemResponse
            {
                Id = item.Id.ToString(),
                Title = request.Language.ToLower() == "ur" ? item.title_ur : item.title_en,
                Text = request.Language.ToLower() == "ur" ? item.text_ur : item.text_en,
                ImageUrl = item.image_url,
                Category = item.category,
                Audience = item.audience,
                Tags = item.tags ?? new List<string>()
            }).ToList();

            return result;
        }
    }

}
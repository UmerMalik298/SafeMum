using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;

namespace SafeMum.Application.Features.Content.GetAllContentGroupItems
{
    public class GetAllContentGroupItemsHandler : IRequestHandler<GetAllContentGroupItemsRequest, List<ContentGroupDto>>
    {
        private readonly Supabase.Client _client;

        public GetAllContentGroupItemsHandler(ISupabaseClientFactory supabaseClient)
        {
            _client = supabaseClient.GetClient();
        }

        public async Task<List<ContentGroupDto>> Handle(GetAllContentGroupItemsRequest request, CancellationToken cancellationToken)
        {
            // Load data from Supabase
            var itemResponse = await _client.From<contentitem>().Get();
            var groupResponse = await _client.From<contentGroups>().Get();

            var allItems = itemResponse.Models;
            var allGroups = groupResponse.Models;

            // Optional: determine requested language (default to "en")
            var language = (request.Language?.ToLower() ?? "en") switch
            {
                "ur" => "ur",
                _ => "en"
            };

            // Final result
            var result = allGroups.Select(group => new ContentGroupDto
            {
                Id = group.Id,
                Title = group.title,
                Description = group.description,
                Audience = group.audience,
                Items = group.contentItemIds != null
                    ? allItems
                        .Where(item => group.contentItemIds.Contains(item.Id))
                        .Select(item => new ContentItemDto
                        {
                            Id = item.Id,
                            Title = language == "ur" ? item.title_ur ?? item.title_en ?? string.Empty : item.title_en ?? string.Empty,
                            Category = item.category ?? string.Empty,
                            Audience = item.audience ?? string.Empty,
                            Summary = language == "ur" ? item.summary_ur ?? item.summary_en ?? string.Empty : item.summary_en ?? string.Empty,
                            Text = language == "ur" ? item.text_ur ?? item.text_en ?? string.Empty : item.text_en ?? string.Empty,
                            ImageUrl = item.image_url ?? string.Empty,
                            Tags = item.tags ?? new List<string>()
                        })
                        .ToList()
                    : new List<ContentItemDto>()
            }).ToList();

            return result;
        }
    }


}

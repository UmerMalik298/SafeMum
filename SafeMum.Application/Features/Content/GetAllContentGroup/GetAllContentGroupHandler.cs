using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;

namespace SafeMum.Application.Features.Content.GetAllContentGroup
{
    public class GetAllContentGroupHandler : IRequestHandler<GetAllContentGroupRequest, List<GetAllContentGroupsResponse>>
    {

        private readonly Supabase.Client _client;

        public GetAllContentGroupHandler(ISupabaseClientFactory supabaseClient)
        {
            _client = supabaseClient.GetClient();
        }
        public async Task<List<GetAllContentGroupsResponse>> Handle(GetAllContentGroupRequest request, CancellationToken cancellationToken)
        {
            await _client.InitializeAsync();

            var table = _client.From<contentGroups>();

            if (!string.IsNullOrEmpty(request.Category))
                table = (Supabase.Interfaces.ISupabaseTable<contentGroups, Supabase.Realtime.RealtimeChannel>)table.Where(x => x.category == request.Category);

            if (!string.IsNullOrEmpty(request.Audience))
                table = (Supabase.Interfaces.ISupabaseTable<contentGroups, Supabase.Realtime.RealtimeChannel>)table.Where(x => x.audience == request.Audience);

            if (!string.IsNullOrEmpty(request.Language))
                table = (Supabase.Interfaces.ISupabaseTable<contentGroups, Supabase.Realtime.RealtimeChannel>)table.Where(x => x.language == request.Language);

            var result = await table.Get();
            var groups = result.Models;

            return groups.Select(g => new GetAllContentGroupsResponse
            {
                Id = g.Id,
                Title = g.title,
                Description = g.description,
                Audience = g.audience,
                Category = g.category,
                Language = g.language,
               
                Tags = g.tags ?? new List<string>(),
                ContentItemIds = g.contentItemIds ?? new List<Guid>()
            }).ToList();
        }
    }
    }

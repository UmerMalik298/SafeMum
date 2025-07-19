using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Communication;

namespace SafeMum.Application.Features.Communication.ChatGroups.GetGroupMessages
{
    public class GetGroupMessagesHandler : IRequestHandler<GetGroupMessagesRequest, List< GetGroupMessagesResponse>>
    {
        private readonly Supabase.Client _client;

        public GetGroupMessagesHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
        }
        public async Task<List<GetGroupMessagesResponse>> Handle(GetGroupMessagesRequest request, CancellationToken cancellationToken)
        {
            var messagesResult = await _client
                .From<Message>()
                .Where(x => x.GroupId == request.Id)
                .Order(x => x.SendAt, Supabase.Postgrest.Constants.Ordering.Ascending) 
                .Get();

            var messages = messagesResult.Models
                .Select(m => new GetGroupMessagesResponse
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    Content = m.Content,
                    SendAt = m.SendAt
                })
                .ToList();

            return messages;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Communication;
using SafeMum.Domain.Entities.Users;

namespace SafeMum.Application.Features.Communication.GetAllConversation
{
    public class GetAllConversationHandler : IRequestHandler<GetAllConversationRequest, List<GetAllConversationResponse>>
    {
        private readonly Supabase.Client _client;
        public GetAllConversationHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
        }

        public async Task<List<GetAllConversationResponse>> Handle(GetAllConversationRequest request, CancellationToken cancellationToken)
        {
            var userId = request.Id;
            var allMessages = await _client
                .From<Message>()
                .Where(x => x.SenderId == userId || x.ReceiverId == userId)
                .Get();

            var grouped = allMessages.Models
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Select(g =>
                {
                    var lastMessage = g.OrderByDescending(m => m.SendAt).First();
                    return new
                    {
                        OtherUserId = g.Key,
                        LastMessage = lastMessage.Content,
                        LastMessageTime = lastMessage.SendAt
                    };
                })
                .ToList();

            var userIds = grouped.Select(g => g.OtherUserId).Distinct().ToList();

            if (userIds.Count == 0)
            {
                return new List<GetAllConversationResponse>();
            }

            // Use fully qualified Operator enum
            var userResults = await _client
                .From<User>()
                .Filter("id", Supabase.Postgrest.Constants.Operator.In, userIds)
                .Get();

            var users = userResults.Models.ToDictionary(u => u.Id, u => u.Username);

            var result = grouped.Select(g => new GetAllConversationResponse
            {
                UserId = g.OtherUserId,
                UserName = users.TryGetValue(g.OtherUserId, out var name) ? name : "Unknown",
                LastMessage = g.LastMessage,
                LastMessageTime = g.LastMessageTime
            })
                .OrderByDescending(c => c.LastMessageTime)
                .ToList();

            return result;
        }
    }
}

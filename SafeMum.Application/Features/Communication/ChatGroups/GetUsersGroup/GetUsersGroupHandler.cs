using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Features.Communication.ChatGroups.GetAllChatGroup;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Communication;

namespace SafeMum.Application.Features.Communication.ChatGroups.GetUsersGroup
{
    public class GetUsersGroupHandler : IRequestHandler<GetUsersGroupRequest, List<GetUsersGroupResponse>>
    {
        private readonly Supabase.Client _client;
        public GetUsersGroupHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
            
        }
        public async Task<List<GetUsersGroupResponse>> Handle(GetUsersGroupRequest request, CancellationToken cancellationToken)
        {
            // Step 1: Get all group memberships
            var groupMemberships = await _client
                .From<GroupMember>()
                .Where(x => x.UserId == request.Id)
                .Get();

            var groupIds = groupMemberships.Models
                                           .Select(m => m.ChatGroupId)
                                           .Distinct()
                                           .ToList();

            if (!groupIds.Any())
                return new List<GetUsersGroupResponse>();

            // Step 2: Get groups for those IDs
            var groupsResult = await _client
                .From<ChatGroup>()
                .Filter("Id", Supabase.Postgrest.Constants.Operator.In, groupIds)
                .Get();

            var groups = groupsResult.Models;

            // Step 3: Get all messages in those groups
            var messagesResult = await _client
                .From<Message>()
                .Filter("groupid", Supabase.Postgrest.Constants.Operator.In, groupIds)
                .Get();

            var messages = messagesResult.Models;

            // Step 4: Build the response with last message
            var response = groups.Select(group =>
            {
                var lastMessage = messages
                    .Where(m => m.GroupId == group.Id)
                    .OrderByDescending(m => m.SendAt)
                    .FirstOrDefault();

                return new GetUsersGroupResponse
                {
                    Name = group.Name,
                    LastMessageContent = lastMessage?.Content,
                    LastMessageTime = lastMessage?.SendAt
                };
            }).ToList();

            return response;
        }


    }
}

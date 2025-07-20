using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Communication;

namespace SafeMum.Application.Features.Communication.ChatGroups.GetAllChatGroup
{
    public class GetAllChatGroupHandler : IRequestHandler<GetAllChatGroupRequest, List<GetAllChatGroupResponse>>
    {
        private readonly Supabase.Client _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllChatGroupHandler(ISupabaseClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _client = clientFactory.GetClient();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<GetAllChatGroupResponse>> Handle(GetAllChatGroupRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
                return new List<GetAllChatGroupResponse>();

           
            var chatGroupsResult = await _client.From<ChatGroup>()
                                                .Where(x => x.AdminUserId == parsedUserId)
                                                .Get();

            var chatGroups = chatGroupsResult.Models;

         
            var groupIds = chatGroups.Select(g => g.Id).ToList();

         
            var groupMembersResult = await _client.From<GroupMember>()
                                                  .Filter("ChatGroupId", Supabase.Postgrest.Constants.Operator.In, groupIds)
                                                  .Get();

            var groupMembers = groupMembersResult.Models;

         
            var messagesResult = await _client.From<Message>()
                                              .Filter("groupid", Supabase.Postgrest.Constants.Operator.In, groupIds)
                                              .Get();

            var messages = messagesResult.Models;

           
            var response = chatGroups.Select(group =>
            {
                var memberIds = groupMembers
                                .Where(m => m.ChatGroupId == group.Id)
                                .Select(m => m.UserId)
                                .ToList();

                var lastMessage = messages
                                  .Where(m => m.GroupId == group.Id)
                                  .OrderByDescending(m => m.SendAt)
                                  .FirstOrDefault();

                return new GetAllChatGroupResponse
                {
                    GroupId = chatGroupsResult.Model.Id,
                    Name = group.Name,
                    MemberUserIds = memberIds,
                    LastMessageContent = lastMessage?.Content,
                    LastMessageTime = lastMessage?.SendAt
                };
            }).ToList();

            return response;
        }

    }
}

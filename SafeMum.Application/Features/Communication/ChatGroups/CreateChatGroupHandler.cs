using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Communication;
using SafeMum.Domain.Entities.Users;
using SafeMum.Infrastructure.Services;

namespace SafeMum.Application.Features.Communication.ChatGroups
{
    public class CreateChatGroupHandler : IRequestHandler<CreateChatGroupRequest, CreateChatGroupResponse>
    {
        private readonly Supabase.Client _client;

        public CreateChatGroupHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();

        }
        public async Task<CreateChatGroupResponse> Handle(CreateChatGroupRequest request, CancellationToken cancellationToken)
        {

            var result = await _client
       .From<User>()
       .Where(u => u.Id == request.AdminUserId)
       .Single();

            var adminUser = result;

            if (adminUser == null || adminUser.Role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can create chat groups.");
            }   


            var groupId = Guid.NewGuid();

            var group = new ChatGroup
            {
                Id = groupId,
                Name = request.Name,
                AdminUserId = request.AdminUserId,
                CreatedAt = DateTime.UtcNow
            };

            // 1. Insert ChatGroup only
            await _client.From<ChatGroup>().Insert(group);

            // 2. Prepare members list
            var members = new List<GroupMember>();

            // Add admin as a member
            members.Add(new GroupMember
            {
                Id = Guid.NewGuid(),
                UserId = request.AdminUserId,
                ChatGroupId = groupId
            });

            // Add other users
            foreach (var userId in request.MemberUserIds.Distinct())
            {
                if (userId == request.AdminUserId) continue;

                members.Add(new GroupMember
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ChatGroupId = groupId
                });
            }

            // 3. Insert members into GroupMember table
            if (members.Any())
            {
                await _client.From<GroupMember>().Insert(members);
            }

            return new CreateChatGroupResponse
            {
                GroupId = groupId
            };
        }

    }
}
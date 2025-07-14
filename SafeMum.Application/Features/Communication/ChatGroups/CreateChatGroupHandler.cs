using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Communication;
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

            var group = new ChatGroup
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                AdminUserId = request.AdminUserId,
                CreatedAt = DateTime.UtcNow
            };

            group.Members.Add(new GroupMember
            {
                Id = Guid.NewGuid(),
                UserId = request.AdminUserId,
                ChatGroupId = group.Id
            });

            foreach (var userId in request.MemberUserIds.Distinct())
            {
             
                if (userId == request.AdminUserId) continue;

                group.Members.Add(new GroupMember
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ChatGroupId = group.Id
                });
            }
          await  _client.From<ChatGroup>().Insert(group);
            return new CreateChatGroupResponse
            {
                GroupId = group.Id
            };


        }
    }
}

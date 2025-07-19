using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Communication.ChatGroups.CreateChatGroup
{
    public class CreateChatGroupRequest : IRequest<CreateChatGroupResponse>
    {
        public string Name { get; set; }
        public Guid AdminUserId { get; set; }
        public List<Guid> MemberUserIds { get; set; } = new();
    }


    public class CreateChatGroupResponse
    {
        public Guid GroupId { get; set; }

    }
}

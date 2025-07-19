using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Communication.ChatGroups.GetAllChatGroup
{
    public class GetAllChatGroupRequest : IRequest<List<GetAllChatGroupResponse>>
    {
    }



    public class GetAllChatGroupResponse
    {
        public string Name { get; set; }
        public List<Guid> MemberUserIds { get; set; }
        public string LastMessageContent { get; set; }
        public DateTime? LastMessageTime { get; set; }

    }
}
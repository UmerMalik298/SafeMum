using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Communication.ChatGroups.GetUsersGroup
{
    public class GetUsersGroupRequest : IRequest<List<GetUsersGroupResponse>>
    {
        public Guid Id { get; set; }
    }

    public class GetUsersGroupResponse
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public string LastMessageContent { get; set; }
        public DateTime? LastMessageTime { get; set; }
    }
}

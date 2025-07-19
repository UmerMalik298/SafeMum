using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Communication.ChatGroups.GetGroupMessages
{
    public class GetGroupMessagesRequest : IRequest<List< GetGroupMessagesResponse>>
    {
        public Guid Id { get; set; }
    }


    public class GetGroupMessagesResponse
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; }
    }

}

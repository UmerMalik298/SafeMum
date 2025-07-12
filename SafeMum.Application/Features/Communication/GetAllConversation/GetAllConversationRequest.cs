using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Communication.GetAllConversation
{
    public class GetAllConversationRequest : IRequest<List<GetAllConversationResponse>>
    {
        public Guid Id { get; set; }
    }
    public class GetAllConversationResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageTime { get; set; }
    }

}

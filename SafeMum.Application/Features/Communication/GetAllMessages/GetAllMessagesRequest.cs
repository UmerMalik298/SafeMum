using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Domain.Entities.Communication;

namespace SafeMum.Application.Features.Communication.GetAllMessages
{
    public class GetAllMessagesRequest : IRequest<List<MessageDto>>
    {
    }


    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; }
        public bool? IsSent { get; set; }
        public bool? IsSeen { get; set; }
    }


}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common.Pagination;

    namespace SafeMum.Application.Features.Communication.GetMessagesByUser
    {
        public class GetMessagesByUserRequest : PaginationRequest, IRequest<PaginatedResponse<MessageDto>>
        {
            public Guid SenderId { get; set; }
            public Guid ReceiverId { get; set; }

            public GetMessagesByUserRequest(Guid senderId, Guid receiverId, int pageNumber = 1, int pageSize = 10)
            {
                SenderId = senderId;
                ReceiverId = receiverId;
                PageNumber = pageNumber;
                PageSize = pageSize;
            }
        }
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


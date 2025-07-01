using MediatR;
using SafeMum.Domain.Entities.Communication;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SafeMum.Application.Repositories;

namespace SafeMum.Application.Features.Communication.GetAllMessages
{
    public class GetAllMessagesHandler : IRequestHandler<GetAllMessagesRequest, List<MessageDto>>
    {
        private readonly IMessageRepository _messageRepository;

        public GetAllMessagesHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<List<MessageDto>> Handle(GetAllMessagesRequest request, CancellationToken cancellationToken)
        {
            var messages = await _messageRepository.GetAllMessagesAsync();

            var result = messages.Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SendAt = m.SendAt,
                IsSent = m.IsSent,
                IsSeen = m.IsSeen
            }).ToList();

            return result;
        }
    }
}



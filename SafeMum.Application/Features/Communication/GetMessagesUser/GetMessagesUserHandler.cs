using MediatR;
using SafeMum.Application.Common.Pagination;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Communication;
using SafeMum.Infrastructure.Services;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.Communication.GetMessagesByUser
{
    public class GetMessagesByUserHandler : IRequestHandler<GetMessagesByUserRequest, PaginatedResponse<MessageDto>>
    {
        private readonly Supabase.Client _client;

        public GetMessagesByUserHandler(ISupabaseClientFactory supabaseClient)
        {
            _client = supabaseClient.GetClient();
            
        }

        public async Task<PaginatedResponse<MessageDto>> Handle(GetMessagesByUserRequest request, CancellationToken cancellationToken)
        {
            var query1 = await _client
                .From<Message>()
                .Where(x => x.SenderId == request.SenderId && x.ReceiverId == request.ReceiverId)
                .Get();

            var query2 = await _client
                .From<Message>()
                .Where(x => x.SenderId == request.ReceiverId && x.ReceiverId == request.SenderId)
                .Get();

            var allMessages = query1.Models
                .Concat(query2.Models)
                .OrderBy(x => x.SendAt)
                .ToList();

            var totalRecords = allMessages.Count;

            var pagedMessages = allMessages
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var result = pagedMessages.Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SendAt = m.SendAt,
                IsSent = m.IsSent,
                IsSeen = m.IsSeen
            }).ToList();

            return new PaginatedResponse<MessageDto>
            {
                Data = result,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = totalRecords
            };
        }

    }
}

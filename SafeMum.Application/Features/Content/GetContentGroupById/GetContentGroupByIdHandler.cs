using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Features.Content.GetContentItemById;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;

namespace SafeMum.Application.Features.Content.GetContentGroupById
{
    public class GetContentGroupByIdHandler : IRequestHandler<GetContentGroupByIdRequest, GetContentGroupByIdResponse>
    {
        private readonly Supabase.Client _client;

        public GetContentGroupByIdHandler(ISupabaseClientFactory supabaseClient)
        {
            _client = supabaseClient.GetClient();
        }
        public async Task<GetContentGroupByIdResponse> Handle(GetContentGroupByIdRequest request, CancellationToken cancellationToken)
        {
            await _client.InitializeAsync();

            var item = await _client.From<contentGroups>().Where(x => x.Id == request.Id).Single();

            if (item == null)
            {

                return new GetContentGroupByIdResponse();
            }
            return new GetContentGroupByIdResponse
            {
                Title = item.title,
                Description = item.description,
                Category = item.category,
                Auidence = item.audience
            };
        }
    }
}

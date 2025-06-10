using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;

namespace SafeMum.Application.Features.Content.CreateContent
{
    public class CreateContentHandler : IRequestHandler<CreateContentRequest, CreateContentResponse>
    {
        private readonly Supabase.Client _client;

        public CreateContentHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();

        }
        public async Task<CreateContentResponse> Handle(CreateContentRequest request, CancellationToken cancellationToken)
        {


            var content = new ContentItem
            {
                Title = request.Title,
                Summary = request.Summary,

            };
            await _client.From<ContentItem>().Insert(content);
            throw new NotImplementedException();
        }
    }
}

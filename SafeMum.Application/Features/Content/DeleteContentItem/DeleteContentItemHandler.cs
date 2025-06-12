using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;

namespace SafeMum.Application.Features.Content.DeleteContentItem
{
    public class DeleteContentItemHandler : IRequestHandler<DeleteContentItemRequest>
    {
        private readonly Supabase.Client _client;

        public DeleteContentItemHandler(ISupabaseClientFactory supabaseClient)
        {
            _client = supabaseClient.GetClient();

        }
        public async Task Handle(DeleteContentItemRequest request, CancellationToken cancellationToken)
        {


            var content = await _client.From<contentitem>().Where(c => c.Id == request.Id).Get();

            if (content == null)
            {
                throw new Exception();
            }
         //   _client.From<ContentItem>().Delete(content);
            throw new NotImplementedException();
        }
    }
}

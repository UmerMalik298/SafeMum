using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;

namespace SafeMum.Application.Features.Content.DeleteContentItem
{
    public class DeleteContentItemHandler : IRequestHandler<DeleteContentItemRequest, Result>
    {
        private readonly Supabase.Client _client;

        public DeleteContentItemHandler(ISupabaseClientFactory supabaseClient)
        {
            _client = supabaseClient.GetClient();

        }
        public async Task<Result> Handle(DeleteContentItemRequest request, CancellationToken cancellationToken)
        {


            var content = await _client.From<contentitem>().Where(c => c.Id == request.Id).Single();

            if (content == null)
            {
                throw new Exception();
            } 
           await  _client.From<contentitem>().Delete(content);
            return Result.Success();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;


namespace SafeMum.Application.Features.Content.CreateContentGroup
{
    public class CreateContentGroupHandler : IRequestHandler<CreateContentGroupRequest, Result>
    {
        private readonly Supabase.Client _client;
        public CreateContentGroupHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();

        }

        public async Task<Result> Handle(CreateContentGroupRequest request, CancellationToken cancellationToken)
        {


            try
            {
                await _client.InitializeAsync();
                var content = new contentGroups
                {


                    Id = Guid.NewGuid(),

                    title = request.Title,
                    description = request.Description,
                    audience = request.Audience,
                    category = request.Category,
                    language = request.Language,
                    tags = request.Tags ?? new List<string>(),
                    contentItemIds = request.ContentItemIds ?? new List<Guid>()
                };

                await _client.From<contentGroups>().Insert(content);



                return Result.Success();

            }
            catch (Exception ex)
            {
                return Result.Failure($"Insert failed: {ex.Message}");



            }

        }
    }
}

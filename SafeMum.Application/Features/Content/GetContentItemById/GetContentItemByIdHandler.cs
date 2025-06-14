using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;

namespace SafeMum.Application.Features.Content.GetContentItemById
{
    public class GetContentItemByIdHandler : IRequestHandler<GetContentItemByIdRequest, GetContentItemByIdResponse>
    {
        private readonly Supabase.Client _client;

        public GetContentItemByIdHandler(ISupabaseClientFactory supabaseClient)
        {
            _client = supabaseClient.GetClient();
        }
        public async Task<GetContentItemByIdResponse> Handle(GetContentItemByIdRequest request, CancellationToken cancellationToken)
        {
         //   await _client.InitializeAsync();

            var item = await _client.From<contentitem>().Where(x => x.Id == request.Id).Single();

            if (item == null)
            {

                return new GetContentItemByIdResponse();
            }


            return new GetContentItemByIdResponse
            {
                Id = item.Id.ToString(),
                TitleEn = item.title_en,
                TitleUr = item.title_ur,
                SummaryEn = item.summary_en,
                SummaryUr = item.summary_ur,
                TextEn = item.text_en,
                TextUr = item.text_ur,
                ImageUrl = item.image_url,
                Category = item.category,
                Audience = item.audience,
                Tags = item.tags ?? new List<string>()
            };
        }
    }
    }


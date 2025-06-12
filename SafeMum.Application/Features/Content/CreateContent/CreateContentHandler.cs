using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;
using Supabase.Postgrest;
using Supabase.Postgrest.Exceptions;

namespace SafeMum.Application.Features.Content.CreateContent
{
    public class CreateContentHandler : IRequestHandler<CreateContentRequest, Result>
    {
        private readonly Supabase.Client _client;

        private readonly ITranslationService _translationService;

        public CreateContentHandler(ISupabaseClientFactory clientFactory, ITranslationService translationService)
        {
            _client = clientFactory.GetClient();
            _translationService = translationService;
        }
        public async Task<Result> Handle(CreateContentRequest request, CancellationToken cancellationToken)
        {

            var titleUrdu = _translationService.TranslateToUrduAsync(request.Title);
            var summryUrdu = _translationService.TranslateToUrduAsync(request.Summary);
            var textUrdu = _translationService.TranslateToUrduAsync(request.Text);

            await Task.WhenAll(titleUrdu, summryUrdu, textUrdu);
            var contentItem = new ContentItem
            {
              Id = Guid.NewGuid(),
               TitleEn = request.Title,
               TitleUr = await titleUrdu,
               SummaryEn = request.Summary,
               SummaryUr = await summryUrdu,
               TextEn = request.Text,
               TextUr = await textUrdu,
               ImageUrl = request.ImageUrl,
               Category = request.Category,
               Audience = request.Audience,
              Tags =  request.Tags
               //Tags = JsonConvert.SerializeObject(request.Tags),
               //CreatedAt = DateTime.UtcNow,




            };
           
                await _client.From<ContentItem>().Insert(contentItem);
                return Result.Success();
            
           
                
              
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;

namespace SafeMum.Application.Features.Content.CreateContentItem
{
    public class CreateContentItemHandler : IRequestHandler<CreateContentItemRequest, Result>
    {
        private readonly Supabase.Client _client;
        private readonly ITranslationService _translationService;
        private readonly IImageUploadService _uploadService;
        public CreateContentItemHandler(ISupabaseClientFactory clientFactory, ITranslationService translationService, IImageUploadService uploadService)
        {
            _client = clientFactory.GetClient();
            _translationService = translationService;
            _uploadService = uploadService;
        }


        public async Task<Result> Handle(CreateContentItemRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _client.InitializeAsync();

        
                var titleUr = await _translationService.TranslateToUrduAsync(request.TitleEn);
                var summaryUr = await _translationService.TranslateToUrduAsync(request.SummaryEn);
                var textUr = await _translationService.TranslateToUrduAsync(request.TextEn);

                var content = new contentitem
                {
                    Id = Guid.NewGuid(),
                    title_en = request.TitleEn, 
                    title_ur = titleUr,
                    summary_en = request.SummaryEn,
                    summary_ur = summaryUr,
                    text_en = request.TextEn,
                    text_ur = textUr,
                    image_url = await _uploadService.UploadImageAsync(request.Image),
                    category = request.Category,
                    audience = request.Audience,
                    tags = request.Tags ?? new List<string>()
                };

                await _client.From<contentitem>().Insert(content);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Insert failed: {ex.Message}");
            }
        }

    }

}


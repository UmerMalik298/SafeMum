using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common.Exceptions;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;
using SafeMum.Domain.Entities.NutritionHealthTracking;
using SafeMum.Domain.Entities.PregnancyInformation;
using SafeMum.Domain.Entities.Users;
using SafeMum.Domain.Entities.WeeklyPregnancyProfile;
using SafeMum.Infrastructure.Services;


namespace SafeMum.Application.Features.Content.GetDashboardInformation
{
    public class GetDashboardInformationHandler : IRequestHandler<GetDashboardInformationRequest, GetDashboardInformationResponse>
    {
        private readonly Supabase.Client _client;
        private readonly IPregnancyTrackerService _pregnancyTrackerService;
        private readonly ITranslationService _translationService;
        public GetDashboardInformationHandler(ISupabaseClientFactory clientFactory, IPregnancyTrackerService pregnancyTrackerService, ITranslationService translationService)
        {
            _client = clientFactory.GetClient();
            _pregnancyTrackerService = pregnancyTrackerService;
            _translationService = translationService;
        }
        public async Task<GetDashboardInformationResponse> Handle(GetDashboardInformationRequest request, CancellationToken cancellationToken)
        {
            await _client.InitializeAsync();

            var user = await _client.From<User>().Where(x => x.Id == request.Id).Single();
            if (user == null)
                throw new ApplicationException("User not found");

            // Determine language (default = English)
            var language = (request.Language?.ToLower() ?? "en") switch
            {
                "ur" => "ur",
                _ => "en"
            };

            // Fetch content items (for dashboard image and tags)
            var contentItem = await _client
                .From<contentitem>()
                .Select("image_url, tags, title_en, title_ur, summary_en, summary_ur, text_en, text_ur")
                .Get();

            var contentItems = contentItem.Models.FirstOrDefault();

            // Fetch pregnancy info
            var pregnancyInfo = await _client.From<UserPregnancyInfo>()
                .Where(x => x.UserId == request.Id).Single()
                ?? throw new AppException("Pregnancy Data Not Found", 404);

            if (pregnancyInfo.LMP == null && pregnancyInfo.EDD == null)
                throw new Exception("User LMP and EDD Not Found");

            int currentWeek = pregnancyInfo.EDD != null
                ? _pregnancyTrackerService.CalculateWeekFromEDD(pregnancyInfo.EDD.Value)
                : _pregnancyTrackerService.CalculateWeekFromLMP(pregnancyInfo.LMP.Value);

            // Fetch weekly profile
            var profile = await _client.From<WeeklyPregnancyProfile>()
                .Where(x => x.WeekNumber == currentWeek).Single();

            // Fetch water intake
            var waterIntake = await _client.From<WaterIntakeLog>()
                .Where(x => x.UserId == request.Id).Single();

            // Safe locals
            var imageUrl = contentItems?.image_url ?? string.Empty;
            var symptoms = contentItems?.tags ?? new List<string>();
            var recommended = profile?.RecommendedActions ?? string.Empty;
            var amountInMl = waterIntake?.AmountInMl ?? 0;

            // --- Language based mapping ---
            string name = user.FirstName ?? string.Empty;
            string bloodGroup = pregnancyInfo.BloodGroup ?? string.Empty;
            string recommendedText = recommended;

            List<string> symptomsList = symptoms ?? new();

            // If Urdu language selected, try to use Urdu or translate
            if (language == "ur")
            {
                // Translate dynamic data
                name = await _translationService.TranslateToUrduAsync(name);
                bloodGroup = await _translationService.TranslateToUrduAsync(bloodGroup);
                recommendedText = await _translationService.TranslateToUrduAsync(recommended);

                var translatedSymptoms = await Task.WhenAll(symptomsList.Select(s => _translationService.TranslateToUrduAsync(s)));
                symptomsList = translatedSymptoms.ToList();
            }

            // Urdu week text if needed
            string currentWeekUrduText = language == "ur" ? $"ہفتہ {currentWeek}" : $"Week {currentWeek}";

            // Return response
            return new GetDashboardInformationResponse
            {
                Name = name,
                BloodGroup = bloodGroup,
                CurrentWeek = currentWeek,
                ImageURL = imageUrl,
                Symptoms = symptomsList,
                RecommendedActions = recommendedText,
                AmountInMl = amountInMl,
                CurrentWeekUrduText = currentWeekUrduText
            };
        }
    }

}

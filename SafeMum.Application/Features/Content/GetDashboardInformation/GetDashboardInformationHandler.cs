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
            var UserId = await _client.From<User>().Where(x => x.Id == request.Id).Single();
            if (UserId == null)
                throw new ApplicationException("User May Not Found");

            var contentItem = await _client
                .From<contentitem>()
                .Select("image_url,tags")
                .Get();

            var contentItems = contentItem.Models.FirstOrDefault();

            var pregnancyInfo = await _client.From<UserPregnancyInfo>()
                .Where(x => x.UserId == request.Id).Single()
                ?? throw new AppException("Pregnancy Data Not Found", 404);

            if (pregnancyInfo.LMP == null && pregnancyInfo.EDD == null)
                throw new Exception("User LMP and EDD Not Found");

            int currentWeek = pregnancyInfo.EDD != null
                ? _pregnancyTrackerService.CalculateWeekFromEDD(pregnancyInfo.EDD.Value)
                : _pregnancyTrackerService.CalculateWeekFromLMP(pregnancyInfo.LMP.Value);

            var profile = await _client.From<WeeklyPregnancyProfile>()
                .Where(x => x.WeekNumber == currentWeek).Single();

            var waterintake = await _client.From<WaterIntakeLog>()
                .Where(x => x.UserId == request.Id).Single();

            // --------- SAFE LOCALS (fix NREs) ----------
            var imageUrl = contentItems?.image_url;                        // may be null -> fine
            var symptoms = contentItems?.tags ?? new List<string>();       // default to empty list
            var recommended = profile?.RecommendedActions;                 // may be null
            var amountInMl = waterintake?.AmountInMl ?? 0;                 // default to 0 when no log
                                                                           // -------------------------------------------

            // Urdu translations (unchanged logic, just use safe locals)
            string? nameUrdu = null;
            if (!string.IsNullOrWhiteSpace(UserId.FirstName))
                nameUrdu = await _translationService.TranslateToUrduAsync(UserId.FirstName);

            string? recommendedUrdu = null;
            if (!string.IsNullOrWhiteSpace(recommended))
                recommendedUrdu = await _translationService.TranslateToUrduAsync(recommended);

            List<string> symptomsUrdu = new();
            if (symptoms.Count > 0)
            {
                var translated = await Task.WhenAll(symptoms.Select(s => _translationService.TranslateToUrduAsync(s)));
                symptomsUrdu = translated.ToList();
            }

            string? currentWeekUrduText = $"ہفتہ {currentWeek}";

            return new GetDashboardInformationResponse
            {
                Name = UserId.FirstName,
                BloodGroup = pregnancyInfo.BloodGroup,
                CurrentWeek = currentWeek,
                ImageURL = imageUrl,                 // use safe local
                Symptoms = symptoms,                 // use safe local
                RecommendedActions = recommended,    // use safe local
                AmountInMl = amountInMl,             // FIX: avoid NRE when null

                NameUrdu = nameUrdu,
                RecommendedActionsUrdu = recommendedUrdu,
                SymptomsUrdu = symptomsUrdu,
                CurrentWeekUrduText = currentWeekUrduText
            };
        }

    }
}
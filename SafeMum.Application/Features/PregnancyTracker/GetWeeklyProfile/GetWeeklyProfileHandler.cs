using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SafeMum.Application.Common;
using SafeMum.Application.Common.Exceptions;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.PregnancyInformation;
using SafeMum.Domain.Entities.Users;
using SafeMum.Domain.Entities.WeeklyPregnancyProfile;
using Supabase.Gotrue;

namespace SafeMum.Application.Features.PregnancyTracker.GetWeeklyProfile
{
    public class GetWeeklyProfileHandler : IRequestHandler<GetWeeklyProfileRequest, WeeklyPregnancyProfileResponse>
    {
        private readonly Supabase.Client _client;
        private readonly IPregnancyTrackerService _pregnancyTrackerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITranslationService _translationService;
        public GetWeeklyProfileHandler(ISupabaseClientFactory clientFactory, IPregnancyTrackerService pregnancyTrackerService, IHttpContextAccessor httpContextAccessor, ITranslationService translationService)
        {
            _client = clientFactory.GetClient();
            _pregnancyTrackerService = pregnancyTrackerService;
            _httpContextAccessor = httpContextAccessor;
            _translationService = translationService;
        }
        public async Task<WeeklyPregnancyProfileResponse> Handle(GetWeeklyProfileRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            
            if (string.IsNullOrWhiteSpace(userId))
                throw new AppException("User Data Not Found", 404);

            Guid parsedGuid = Guid.Parse(userId);
            var pregnancyInfo = await _client.From<UserPregnancyInfo>().Where(x => x.UserId == parsedGuid).Single() ?? 
                throw new AppException("Pregnancy Data Not Found", 404);


            if (pregnancyInfo.LMP == null && pregnancyInfo.EDD == null) throw new Exception("User LMP and EDD Not Found");


            int currentWeek = pregnancyInfo.EDD != null ? _pregnancyTrackerService.CalculateWeekFromEDD(pregnancyInfo.EDD.Value) :
                _pregnancyTrackerService.CalculateWeekFromLMP(pregnancyInfo.LMP.Value);

            var profile = await _client.From<WeeklyPregnancyProfile>().Where(x => x.WeekNumber == currentWeek).Single() ;

            if (profile == null)
            {
                throw new AppException("Profile Not Found", 404);
            }

            var language = (request.Language?.ToLower() ?? "en") switch
            {
                "ur" => "ur",
                _ => "en"
            };
            string? babyDevUr = null, motherChangesUr = null, nutritionTipsUr = null, dangerSignsUr = null, recommendedActionsUr = null;

            if (language == "ur")
            {
                babyDevUr = await _translationService.TranslateToUrduAsync(profile.BabyDevelopment);
                motherChangesUr = await _translationService.TranslateToUrduAsync(profile.EmotionalChanges);
                nutritionTipsUr = await _translationService.TranslateToUrduAsync(profile.NutritionTips);
                dangerSignsUr = await _translationService.TranslateToUrduAsync(profile.DangerSigns);
                recommendedActionsUr = await _translationService.TranslateToUrduAsync(profile.RecommendedActions);
            }

          

            string babyDevelopment, motherChanges, nutritionTips, dangerSigns, recommendedActions;

            if (language == "ur")
            {
                babyDevelopment = await _translationService.TranslateToUrduAsync(profile.BabyDevelopment);
                motherChanges = $"{profile.PhysicalChanges}\n{await _translationService.TranslateToUrduAsync(profile.EmotionalChanges)}";
                nutritionTips = await _translationService.TranslateToUrduAsync(profile.NutritionTips);
                dangerSigns = await _translationService.TranslateToUrduAsync(profile.DangerSigns);
                recommendedActions = await _translationService.TranslateToUrduAsync(profile.RecommendedActions);
            }
            else
            {
                babyDevelopment = profile.BabyDevelopment;
                motherChanges = $"{profile.PhysicalChanges}\n{profile.EmotionalChanges}";
                nutritionTips = profile.NutritionTips;
                dangerSigns = profile.DangerSigns;
                recommendedActions = profile.RecommendedActions;
            }

            return new WeeklyPregnancyProfileResponse
            {
                WeekNumber = profile.WeekNumber,
                BabyDevelopment = babyDevelopment,
                MotherChanges = motherChanges,
                NutritionTips = nutritionTips,
                DangerSigns = dangerSigns,
                RecommendedActions = recommendedActions
            };





        }
    }
}

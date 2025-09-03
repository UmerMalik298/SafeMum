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


            var pregnancyInfo = await _client.From<UserPregnancyInfo>().Where(x => x.UserId == userId).Single() ?? 
                throw new AppException("Pregnancy Data Not Found", 404);


            if (pregnancyInfo.LMP == null && pregnancyInfo.EDD == null) throw new Exception("User LMP and EDD Not Found");


            int currentWeek = pregnancyInfo.EDD != null ? _pregnancyTrackerService.CalculateWeekFromEDD(pregnancyInfo.EDD.Value) :
                _pregnancyTrackerService.CalculateWeekFromLMP(pregnancyInfo.LMP.Value);

            var profile = await _client.From<WeeklyPregnancyProfile>().Where(x => x.WeekNumber == currentWeek).Single() ;

            if (profile == null)
            {
                throw new AppException("Profile Not Found", 404);
            }

           
            var BabyDevelopmentUrdu = await _translationService.TranslateToUrduAsync(profile.BabyDevelopment);
            var MotherChangesUrdu = await _translationService.TranslateToUrduAsync(profile.EmotionalChanges);
            var NutritionTipsUrdu = await _translationService.TranslateToUrduAsync(profile.NutritionTips);
            var DangerSignsUrdu = await _translationService.TranslateToUrduAsync(profile.DangerSigns);
            var RecommendedActionsUrdu =  await _translationService.TranslateToUrduAsync(profile.RecommendedActions);
            return new WeeklyPregnancyProfileResponse
            {
                WeekNumber = profile.WeekNumber,
                BabyDevelopment = profile.BabyDevelopment,
                MotherChanges = $"{profile.PhysicalChanges}\n{profile.EmotionalChanges}",
                NutritionTips = profile.NutritionTips,
                DangerSigns = profile.DangerSigns,
                RecommendedActions = profile.RecommendedActions,


               
                BabyDevelopmentUr = BabyDevelopmentUrdu ,
                MotherChangesUr = MotherChangesUrdu,
                NutritionTipsUr = NutritionTipsUrdu,
                DangerSignsUr = DangerSignsUrdu,
                RecommendedActionsUr = RecommendedActionsUrdu


            };

       

        }
    }
}

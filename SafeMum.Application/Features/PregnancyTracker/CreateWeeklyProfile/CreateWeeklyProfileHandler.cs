using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.WeeklyPregnancyProfile;

namespace SafeMum.Application.Features.PregnancyTracker.CreateWeeklyProfile
{
    public class CreateWeeklyProfileHandler : IRequestHandler<CreateWeeklyProfileRequest, Result>
    {
        private readonly Supabase.Client _client;
        public CreateWeeklyProfileHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
            
        }
        public async Task<Result> Handle(CreateWeeklyProfileRequest request, CancellationToken cancellationToken)
        {

            try
            {
                var weeklyProfile = new WeeklyPregnancyProfile
                {
                    WeekNumber = request.WeekNumber,
                    BabyDevelopment = request.BabyDevelopment,
                    PhysicalChanges = request.PhysicalChanges,
                    EmotionalChanges = request.EmotionalChanges,
                    NutritionTips = request.NutritionTips,
                    DangerSigns = request.DangerSigns,
                    RecommendedActions = request.RecommendedActions,
                    CreatedAt = DateTime.UtcNow
                };

                var response = await _client
                    .From<WeeklyPregnancyProfile>()
                    .Insert(weeklyProfile);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure("Faild to insert");
            }
        
    }
    }
}

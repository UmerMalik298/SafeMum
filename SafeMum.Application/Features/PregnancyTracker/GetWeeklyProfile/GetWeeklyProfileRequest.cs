using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.PregnancyTracker.GetWeeklyProfile
{
    public class GetWeeklyProfileRequest : IRequest<WeeklyPregnancyProfileResponse>
    {
        public string? Language { get; set; } = "en";
    }
    public class WeeklyPregnancyProfileResponse
    {
        public int WeekNumber { get; set; }
        public string BabyDevelopment { get; set; }
        public string MotherChanges { get; set; }
        public string NutritionTips { get; set; }
        public string DangerSigns { get; set; }
        public string RecommendedActions { get; set; }



     
        //public string? BabyDevelopmentUr { get; set; }
        //public string? MotherChangesUr { get; set; }
        //public string? NutritionTipsUr { get; set; }
        //public string? DangerSignsUr { get; set; }
        //public string? RecommendedActionsUr { get; set; }
    }

}

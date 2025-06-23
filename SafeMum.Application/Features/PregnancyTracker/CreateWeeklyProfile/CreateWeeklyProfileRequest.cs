using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.PregnancyTracker.CreateWeeklyProfile
{
    public class CreateWeeklyProfileRequest : IRequest<Result>
    {
        public int WeekNumber { get; set; }
        public string? BabyDevelopment { get; set; }
        public string? PhysicalChanges { get; set; }
        public string? EmotionalChanges { get; set; }
        public string? NutritionTips { get; set; }
        public string? DangerSigns { get; set; }
        public string? RecommendedActions { get; set; }
    }

}

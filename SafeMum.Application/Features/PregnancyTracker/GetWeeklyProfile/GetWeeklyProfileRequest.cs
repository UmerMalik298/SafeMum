﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.PregnancyTracker.GetWeeklyProfile
{
    public class GetWeeklyProfileRequest : IRequest<WeeklyPregnancyProfileResponse>
    {
    }
    public class WeeklyPregnancyProfileResponse
    {
        public int WeekNumber { get; set; }
        public string BabyDevelopment { get; set; }
        public string MotherChanges { get; set; }
        public string NutritionTips { get; set; }
        public string DangerSigns { get; set; }
        public string RecommendedActions { get; set; }
    }

}

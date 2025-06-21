using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.Common;

namespace SafeMum.Domain.Entities.WeeklyPregnancyProfile
{
    public class WeeklyPregnancyProfile : BaseEntity
    {
        public int WeekNumber { get; set; }
        public string BabyDevelopment { get; set; }
        public string MotherChanges { get; set; }
        public string NutritionTips { get; set; }
        public string DangerSigns { get; set; }
        public string RecommendedActions { get; set; }
    }

}

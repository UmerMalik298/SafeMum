using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.Common;
using Supabase.Postgrest.Attributes;

namespace SafeMum.Domain.Entities.WeeklyPregnancyProfile
{
    [Table("weeklypregnancyprofile")]
    public class WeeklyPregnancyProfile : BaseEntity
    {

        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("weeknumber")]
        public int WeekNumber { get; set; }
        [Column("babydevelopment")]
        public string? BabyDevelopment { get; set; }
        [Column("physicalchanges")]
        public string? PhysicalChanges { get; set; }
        [Column("emotionalchanges")]
        public string? EmotionalChanges { get; set; }
        [Column("nutritiontips")]   
        public string? NutritionTips { get; set; }
        [Column("dangersigns")]
        public string? DangerSigns { get; set; }

        [Column("recommendedactions")]
        public string? RecommendedActions { get; set; }


    }

}

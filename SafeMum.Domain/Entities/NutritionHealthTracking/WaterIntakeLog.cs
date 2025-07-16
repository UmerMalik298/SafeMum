using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.NutritionHealthTracking
{

    [Table("WaterIntakeLog")]
    public class WaterIntakeLog : BaseModel
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("userId")]
        public Guid UserId { get; set; }
        [Column("amountInMl")]
        public int AmountInMl { get; set; }


        [Column("consumedAt")]
        public DateTime ConsumedAt { get; set; }
    }
}

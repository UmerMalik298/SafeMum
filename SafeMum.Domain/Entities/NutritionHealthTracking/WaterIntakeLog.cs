using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.NutritionHealthTracking
{
    public class WaterIntakeLog : BaseModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int AmountInMl { get; set; }
        public DateTime ConsumedAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.NutritionHealthTracking
{
    public class SupplementLog : BaseModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }  
        public string Name { get; set; }
        public string Dosage { get; set; }
        public DateTime TakenAt { get; set; }
    }
}

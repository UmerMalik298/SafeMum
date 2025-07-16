using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.NutritionHealthTracking
{

    [Table("SupplementLog")]
    public class SupplementLog : BaseModel
    {

        [Column("id")]
        public Guid Id { get; set; }


        [Column("userId")]
        public Guid UserId { get; set; }


        [Column("name")]
        public string Name { get; set; }


        [Column("dosage")]
        public string Dosage { get; set; }


        [Column("takenAt")]
        public DateTime TakenAt { get; set; }
    }
}

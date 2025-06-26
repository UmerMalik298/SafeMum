using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using SafeMum.Domain.Entities.Common;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.PregnancyInformation
{

    [Supabase.Postgrest.Attributes.Table("userpregnancyinfo")]
    public class UserPregnancyInfo : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Supabase.Postgrest.Attributes.Column("userid")]
        public string UserId { get; set; }

        [Supabase.Postgrest.Attributes.Column("iscurrentlypregnant")]
        public bool? IsCurrentlyPregnant { get; set; } = true;


        [Supabase.Postgrest.Attributes.Column("lmp")]
        public DateTime? LMP { get; set; }


        [Supabase.Postgrest.Attributes.Column("edd")]
        public DateTime? EDD { get; set; }


        [Supabase.Postgrest.Attributes.Column("gravida")]
        public int? Gravida { get; set; }   // Previous pregnancies


        [Supabase.Postgrest.Attributes.Column("parity")]
        public int? Parity { get; set; }    // Live births


        [Supabase.Postgrest.Attributes.Column("healthconditions")]
        public List<string>? HealthConditions { get; set; }  // Anemia, etc.


        [Supabase.Postgrest.Attributes.Column("preferredlanguage")]
        public string? PreferredLanguage { get; set; }       // Urdu, etc.

        [Supabase.Postgrest.Attributes.Column("literacylevel")]
        public string? LiteracyLevel { get; set; }           // 'text' or 'voice'

        [Supabase.Postgrest.Attributes.Column("emergencycontactname")]
        public string? EmergencyContactName { get; set; }

        [Supabase.Postgrest.Attributes.Column("emergencycontactnumber")]
        public string? EmergencyContactNumber { get; set; }

        [Supabase.Postgrest.Attributes.Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [Supabase.Postgrest.Attributes.Column("isdiabetic")]
        public bool? IsDiabetic { get; set; }



        [Supabase.Postgrest.Attributes.Column("hashypertension")]
        public bool? HasHypertension { get; set; }



        [Supabase.Postgrest.Attributes.Column("haemoglobinlevel")]
        public float? HaemoglobinLevel { get; set; }



        [Supabase.Postgrest.Attributes.Column("issmoker")]
        public bool? IsSmoker { get; set; }


        [Supabase.Postgrest.Attributes.Column("takesmedication")]
        public List<string>? TakesMedication { get; set; }



        [Supabase.Postgrest.Attributes.Column("bloodgroup")]
        public string? BloodGroup { get; set; }



    }

}

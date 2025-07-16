using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.NutritionHealthTracking
{


    [Table("PrenatalAppointment")]
    public class PrenatalAppointment : BaseModel
    {


        [Column("id")]
        public Guid Id { get; set; }

        [Column("userId")]
        public Guid UserId { get; set; }


        [Column("doctorName")]
        public string DoctorName { get; set; }

        [Column("hospitalNamae")]
        public string HospitalNamae { get; set; }

        [Column("appointmentDate")]
        public DateTime AppointmentDate { get; set; }

        [Column("location")]
        public string Location { get; set; }
    }
}

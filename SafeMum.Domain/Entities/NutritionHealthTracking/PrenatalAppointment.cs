using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.NutritionHealthTracking
{
    public class PrenatalAppointment : BaseModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DoctorName { get; set; }
        public string HospitalNamae { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Location { get; set; }
    }
}

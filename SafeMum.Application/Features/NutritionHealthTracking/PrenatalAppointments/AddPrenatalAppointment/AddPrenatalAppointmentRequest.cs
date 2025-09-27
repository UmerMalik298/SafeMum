using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.NutritionHealthTracking.PrenatalAppointments.AddPrenatalAppointment
{
    public class AddPrenatalAppointmentRequest : IRequest<Result>
    {
        public string DoctorName { get; set; }
        public string HospitalNamae { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Location { get; set; }
    }
}

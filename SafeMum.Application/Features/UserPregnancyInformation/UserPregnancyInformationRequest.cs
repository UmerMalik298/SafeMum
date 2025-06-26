using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.UserPregnancyInformation
{
    public class UserPregnancyInformationRequest : IRequest<Result>
    {
        public bool? CurrentlyPregnant { get; set; }
        public DateOnly? EDD { get; set; }

        public int? NoOfPreviousPregnancies { get; set; }

        public int? NoOfLiveBirths { get; set; }

        public string? EmergencyContactName { get; set; }

        public string? EmergencyContactNumber { get; set; }



        public bool? IsDiabetic { get; set; }

        public bool? HasHypertension { get; set; }

        public float? HaemoglobinLevel { get; set; }

        public bool? IsSmoker { get; set; }

        public List<string>? TakesMedication { get; set; }

        public string? BloodGroup { get; set; }
    }
}

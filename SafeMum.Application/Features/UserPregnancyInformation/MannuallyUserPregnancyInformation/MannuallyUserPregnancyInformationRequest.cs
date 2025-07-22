using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.UserPregnancyInformation.MannuallyUserPregnancyInformation
{
    public class MannuallyUserPregnancyInformationRequest : IRequest<Result>
    {
        public string? Name { get; set; }

        public string? HusbandName { get; set; }

        public string? Address { get; set; }

        public int? NumberOfMiscarriages { get; set; }

        public string? CNIC { get; set; }



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

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
        public bool CurrentPregnant { get; set; }
        public DateOnly? EDD { get; set; }

        public int NoOfPreviousPregnancies { get; set; }

        public int NoOfLiveBirths { get; set; }

        public string EmergencyContactName { get; set; }

        public string EmergencyContactNumber { get; set; }
    }
}

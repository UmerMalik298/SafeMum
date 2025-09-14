using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Content.GetDashboardInformation
{
    public class GetDashboardInformationRequest : IRequest<GetDashboardInformationResponse>
    {
        public Guid Id { get; set; }
    }
    public class GetDashboardInformationResponse
    {

        public string Name { get; set; }
        public int? CurrentWeek { get; set; }
        public string? ImageURL { get; set; }

        public string BloodGroup { get; set; }

        public string? RecommendedActions { get; set; }
        public List<string> Symptoms { get; set; }

        public int AmountInMl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common.Exceptions;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.PregnancyInformation;
using SafeMum.Domain.Entities.Users;
using SafeMum.Domain.Entities.WeeklyPregnancyProfile;
using SafeMum.Infrastructure.Services;


namespace SafeMum.Application.Features.Content.GetDashboardInformation
{
    public class GetDashboardInformationHandler : IRequestHandler<GetDashboardInformationRequest, GetDashboardInformationResponse>
    {
        private readonly Supabase.Client _client;
        private readonly IPregnancyTrackerService _pregnancyTrackerService;
        public GetDashboardInformationHandler(ISupabaseClientFactory clientFactory, IPregnancyTrackerService pregnancyTrackerService)
        {
            _client = clientFactory.GetClient();
            _pregnancyTrackerService = pregnancyTrackerService;
        }
        public async Task<GetDashboardInformationResponse> Handle(GetDashboardInformationRequest request, CancellationToken cancellationToken)
        {
            var UserId = await _client.From<User>().Single();
            if (UserId == null)
            {

                throw new ApplicationException("User May Not Found");
            }

            var pregnancyInfo = await _client.From<UserPregnancyInfo>().Where(x => x.UserId == request.Id.ToString()).Single() ??
            throw new AppException("Pregnancy Data Not Found", 404);


            if (pregnancyInfo.LMP == null && pregnancyInfo.EDD == null) throw new Exception("User LMP and EDD Not Found");


            int currentWeek = pregnancyInfo.EDD != null ? _pregnancyTrackerService.CalculateWeekFromEDD(pregnancyInfo.EDD.Value) :
                _pregnancyTrackerService.CalculateWeekFromLMP(pregnancyInfo.LMP.Value);

            var profile = await _client.From<WeeklyPregnancyProfile>().Where(x => x.WeekNumber == currentWeek).Single();


            return new GetDashboardInformationResponse
            {
                
            };
        }
    }
}

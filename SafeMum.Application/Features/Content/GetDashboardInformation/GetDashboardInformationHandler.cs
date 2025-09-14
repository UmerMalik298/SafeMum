using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common.Exceptions;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Content;
using SafeMum.Domain.Entities.NutritionHealthTracking;
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
            var UserId = await _client.From<User>().Where(x=>x.Id== request.Id).Single();
            if (UserId == null)
            {

                throw new ApplicationException("User May Not Found");
            }
            var contentItem = await _client
      .From<contentitem>()
      .Select("image_url,tags")
      .Get();
            var contentItems = contentItem.Models.FirstOrDefault();
            var pregnancyInfo = await _client.From<UserPregnancyInfo>().Where(x => x.UserId == request.Id).Single() ??
            throw new AppException("Pregnancy Data Not Found", 404);


            if (pregnancyInfo.LMP == null && pregnancyInfo.EDD == null) throw new Exception("User LMP and EDD Not Found");


            int currentWeek = pregnancyInfo.EDD != null ? _pregnancyTrackerService.CalculateWeekFromEDD(pregnancyInfo.EDD.Value) :
                _pregnancyTrackerService.CalculateWeekFromLMP(pregnancyInfo.LMP.Value);

            var profile = await _client.From<WeeklyPregnancyProfile>().Where(x => x.WeekNumber == currentWeek).Single();
            var waterintake = await _client.From<WaterIntakeLog>().Where(x => x.UserId == request.Id).Single();

            return new GetDashboardInformationResponse
            {
                Name = UserId.FirstName,
                BloodGroup = pregnancyInfo.BloodGroup,
                CurrentWeek = currentWeek,
                ImageURL = contentItems.image_url,
                Symptoms = contentItems.tags,
                RecommendedActions = profile.RecommendedActions,
                AmountInMl = waterintake.AmountInMl,


            };
        }
    }
}

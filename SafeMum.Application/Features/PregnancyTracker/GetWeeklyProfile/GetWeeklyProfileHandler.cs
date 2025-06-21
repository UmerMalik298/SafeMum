//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MediatR;
//using SafeMum.Application.Common;
//using SafeMum.Application.Interfaces;
//using SafeMum.Domain.Entities.Users;
//using Supabase.Gotrue;

//namespace SafeMum.Application.Features.PregnancyTracker.GetWeeklyProfile
//{
//    public class GetWeeklyProfileHandler : IRequestHandler<GetWeeklyProfileRequest, WeeklyPregnancyProfileResponse>
//    {
//        private readonly Supabase.Client _client;
//        private readonly IPregnancyTrackerService _pregnancyTrackerService;
//        public GetWeeklyProfileHandler(ISupabaseClientFactory clientFactory, IPregnancyTrackerService pregnancyTrackerService)
//        {
//            _client = clientFactory.GetClient();
//            _pregnancyTrackerService = pregnancyTrackerService;
//        }
//        public async Task<WeeklyPregnancyProfileResponse> Handle(GetWeeklyProfileRequest request, CancellationToken cancellationToken)
//        {
//            var userId = await _client.From<User>().Single();
//            if (userId == null || (user.EDD == null && user.LMP == null))
//            {
//                return Result.Failure("User Data Not Found");
//            }
//            int currentWeek = user.EDD != null
//            ? _tracker.CalculateWeekFromEDD(user.EDD.Value)
//            : _tracker.CalculateWeekFromLMP(user.LMP.Value);
//        }
//    }
//}

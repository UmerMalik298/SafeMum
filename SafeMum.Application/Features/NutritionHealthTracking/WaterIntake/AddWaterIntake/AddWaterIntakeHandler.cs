using MediatR;
using Microsoft.AspNetCore.Http;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Common;
using SafeMum.Domain.Entities.NutritionHealthTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.NutritionHealthTracking.WaterIntake.AddWaterIntake
{
    public class AddWaterIntakeHandler : IRequestHandler<AddWaterIntakeRequest, Result>
    {
        private readonly Supabase.Client _client;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IInAppNotificationService _inAppNotificationService;
        private readonly IPushNotificationService _notificationService;

        public AddWaterIntakeHandler(
            ISupabaseClientFactory clientFactory,
            IHttpContextAccessor contextAccessor,
            IInAppNotificationService inAppNotificationService,
            IPushNotificationService notificationService)
        {
            _client = clientFactory.GetClient();
            _contextAccessor = contextAccessor;
            _inAppNotificationService = inAppNotificationService;
            _notificationService = notificationService;
        }

        public async Task<Result> Handle(AddWaterIntakeRequest request, CancellationToken cancellationToken)
        {
            var userIdStr = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdStr))
                return Result.Failure("Unauthorized.");

            var userId = Guid.Parse(userIdStr);

            var twentyFourHoursAgo = DateTime.UtcNow.AddHours(-24);

            var existingRes = await _client
     .From<WaterIntakeLog>()
     .Where(x => x.UserId == userId && x.ConsumedAt >= twentyFourHoursAgo)
     .Get();

            if (existingRes.Models.Any())
                return Result.Failure("You have already logged water intake in the last 24 hours.");

            var log = new WaterIntakeLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AmountInMl = request.AmountInMl,
                ConsumedAt = DateTime.UtcNow,
            };

            await _client.From<WaterIntakeLog>().Insert(log);

           
            await _inAppNotificationService.CreateNotificationAsync(
                userId,
                "Water Intake Logged",
                $"You logged {request.AmountInMl}ml of water today. Stay hydrated 💧",
                "water",
                new { intakeId = log.Id }
            );
            var tokensRes = await _client
    .From<DeviceToken>()
    .Filter("userid", Supabase.Postgrest.Constants.Operator.Equals, userId.ToString())
    .Get();

            var tokens = tokensRes.Models?
                .Select(t => t.Token)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToList();

            if (tokens != null && tokens.Any())
            {
                var title = "Water Intake Logged";
                var body = $"You logged {request.AmountInMl}ml of water today. 💧";

                var sendTasks = tokens.Select(token =>
                    _notificationService.SendPushNotification(
                        token,
                        title,
                        body,
                        new
                        {
                            type = "water",
                            intakeId = log.Id.ToString(),
                            amountInMl = request.AmountInMl
                        }
                    )
                );

                await Task.WhenAll(sendTasks);
            }

            return Result.Success();
        }
    }
}

using Hangfire.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.AppNotification;
using SafeMum.Domain.Entities.Common;
using SafeMum.Domain.Entities.NutritionHealthTracking;
using SafeMum.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.NutritionHealthTracking.Supplement.AddSupplement
{
    public class AddSupplementHandler : IRequestHandler<AddSupplementRequest, Result>
    {
        private readonly Supabase.Client _client;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IInAppNotificationService _inAppNotifications;
        private readonly ILogger<AddSupplementHandler> _logger;
        private readonly IPushNotificationService _notificationService;

        public AddSupplementHandler(
            ISupabaseClientFactory clientFactory,
            IHttpContextAccessor contextAccessor,
            IInAppNotificationService inAppNotifications,
            ILogger<AddSupplementHandler> logger,
            IPushNotificationService notificationService)
        {
            _client = clientFactory.GetClient();
            _contextAccessor = contextAccessor;
            _inAppNotifications = inAppNotifications;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task<Result> Handle(AddSupplementRequest request, CancellationToken cancellationToken)
        {
            var userIdStr = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdStr))
                return Result.Failure("Unauthorised.");

            var userId = Guid.Parse(userIdStr);

            // Check if a log exists in the last 24h (avoid .Single() which throws when 0 rows)
            var cutoff = DateTime.UtcNow.AddHours(-24);
            var existingRes = await _client
                .From<SupplementLog>()
                .Where(x => x.UserId == userId && x.TakenAt >= cutoff)
                .Get();

            if (existingRes.Models.Any())
                return Result.Failure("You have already added a supplement intake log in the last 24 hours.");

            // Save the new supplement log
            var log = new SupplementLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = request.Name,
                Dosage = request.Dosage,
                TakenAt = DateTime.UtcNow
            };

            await _client.From<SupplementLog>().Insert(log);

            // In-app notification (DB + SignalR via your InAppNotificationService)
            await _inAppNotifications.CreateNotificationAsync(
                userId,
                "Supplement logged",
                $"You took {request.Name} {request.Dosage}. Drink a glass of water 💧",
                type: "supplement",
                data: new { supplementLogId = log.Id, name = request.Name }
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
                var title = "Supplement Logged";
                var body = $"You took {request.Name} {request.Dosage}.";

                var sendTasks = tokens.Select(token =>
                    _notificationService.SendPushNotification(
                        token,
                        title,
                        body,
                        new
                        {
                            type = "supplement",
                            supplementLogId = log.Id.ToString(),
                            name = request.Name
                        }
                    )
                );

                await Task.WhenAll(sendTasks);
            }

            _logger.LogInformation("Supplement logged and in-app notification created for user {UserId}", userId);
            return Result.Success();
        }
    }
}

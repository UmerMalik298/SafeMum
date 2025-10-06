using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.InAppNotification.CreateInAppNotification
{
    public class CreateInAppNotificationHandler
         : IRequestHandler<CreateInAppNotificationRequest, Result>
    {
        private readonly IInAppNotificationService _inAppNotificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateInAppNotificationHandler> _logger;

        public CreateInAppNotificationHandler(
            IInAppNotificationService inAppNotificationService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CreateInAppNotificationHandler> logger)
        {
            _inAppNotificationService = inAppNotificationService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Result> Handle(CreateInAppNotificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Resolve target user
                Guid userId;
                if (request.UserId.HasValue)
                {
                    userId = request.UserId.Value;
                }
                else
                {
                    var userIdStr = _httpContextAccessor.HttpContext?
                        .User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (string.IsNullOrWhiteSpace(userIdStr))
                        return Result.Failure("Unauthorised: user id not found in token.");

                    userId = Guid.Parse(userIdStr);
                }

                // Validate basic input
                if (string.IsNullOrWhiteSpace(request.Title))
                    return Result.Failure("Title is required.");
                if (string.IsNullOrWhiteSpace(request.Message))
                    return Result.Failure("Message is required.");

                // Create the in-app notification (saves to DB + SignalR broadcast via gateway)
                await _inAppNotificationService.CreateNotificationAsync(
                    userId,
                    request.Title,
                    request.Message,
                    request.Type ?? "general",
                    request.Data
                );

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create in-app notification");
                return Result.Failure("Failed to create in-app notification.");
            }
        }
    }
}
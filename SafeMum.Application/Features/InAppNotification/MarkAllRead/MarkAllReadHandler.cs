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

namespace SafeMum.Application.Features.InAppNotification.MarkAllRead
{
    public class MarkAllReadHandler : IRequestHandler<MarkAllReadRequest, Result>
    {
        private readonly IInAppNotificationService _service;
        private readonly IHttpContextAccessor _http;
        private readonly ILogger<MarkAllReadHandler> _logger;

        public MarkAllReadHandler(
            IInAppNotificationService service,
            IHttpContextAccessor http,
            ILogger<MarkAllReadHandler> logger)
        {
            _service = service;
            _http = http;
            _logger = logger;
        }
        public async Task<Result> Handle(MarkAllReadRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var idStr = _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(idStr)) return Result.Failure("Unauthorised.");
                var userId = Guid.Parse(idStr);

                var ok = await _service.MarkAllAsReadAsync(userId);
                return ok ? Result.Success() : Result.Failure("No notifications to update.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MarkAllRead failed");
                return Result.Failure("Failed to mark all as read.");
            }
        }
    }
}

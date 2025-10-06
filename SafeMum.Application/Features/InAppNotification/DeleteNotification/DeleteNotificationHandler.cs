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

namespace SafeMum.Application.Features.InAppNotification.DeleteNotification
{
    public class DeleteNotificationHandler : IRequestHandler<DeleteNotificationRequest, Result>
    {
        private readonly IInAppNotificationService _service;
        private readonly IHttpContextAccessor _http;
        private readonly ILogger<DeleteNotificationHandler> _logger;

        public DeleteNotificationHandler(
            IInAppNotificationService service,
            IHttpContextAccessor http,
            ILogger<DeleteNotificationHandler> logger)
        {
            _service = service;
            _http = http;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteNotificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var idStr = _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(idStr)) return Result.Failure("Unauthorised.");
                var userId = Guid.Parse(idStr);

                var ok = await _service.DeleteNotificationAsync(request.NotificationId, userId);
                return ok ? Result.Success() : Result.Failure("Not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteNotification failed");
                return Result.Failure("Failed to delete notification.");
            }
        }
    }
}

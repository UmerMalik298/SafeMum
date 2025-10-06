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

namespace SafeMum.Application.Features.InAppNotification.GetUnreadCount
{
    public class GetUnreadCountHandler : IRequestHandler<GetUnreadCountRequest, Result<int>>
    {
        private readonly IInAppNotificationService _service;
        private readonly IHttpContextAccessor _http;
        private readonly ILogger<GetUnreadCountHandler> _logger;

        public GetUnreadCountHandler(
            IInAppNotificationService service,
            IHttpContextAccessor http,
            ILogger<GetUnreadCountHandler> logger)
        {
            _service = service;
            _http = http;
            _logger = logger;
        }
        public async Task<Result<int>> Handle(GetUnreadCountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var idStr = _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(idStr)) return Result.Failure<int>("Unauthorised.");
                var userId = Guid.Parse(idStr);

                var count = await _service.GetUnreadCountAsync(userId);
                return Result.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUnreadCount failed");
                return Result.Failure<int>("Failed to get unread count.");
            }
        }
    }
}

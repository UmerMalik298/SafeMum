using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace SafeMum.API.Hubs
{
    namespace SafeMum.API.Hubs
    {
        public class NotificationHub : Hub
        {
            private readonly ILogger<NotificationHub> _logger;

            public NotificationHub(ILogger<NotificationHub> logger)
            {
                _logger = logger;
            }

            public async Task JoinUserNotifications(string userId)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
                _logger.LogInformation("User {UserId} joined notification group", userId);
            }

            public async Task LeaveUserNotifications(string userId)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
                _logger.LogInformation("User {UserId} left notification group", userId);
            }

            public override async Task OnConnectedAsync()
            {
                var userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
                }
                await base.OnConnectedAsync();
            }

            public override async Task OnDisconnectedAsync(Exception exception)
            {
                var userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
                }
                await base.OnDisconnectedAsync(exception);
            }
        }
    }
}
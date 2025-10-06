using Microsoft.AspNetCore.SignalR;
using SafeMum.API.Hubs.SafeMum.API.Hubs;
using SafeMum.Application.Interfaces;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace SafeMum.API.Hubs
{

    public class SignalRNotificationGateway : INotificationGateway
    {
        private readonly IHubContext<NotificationHub> _hub;

        public SignalRNotificationGateway(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public Task SendToUserAsync(Guid userId, string method, object payload)
        {
            // Clients are grouped as "user_{id}" by the hub
            return _hub.Clients.Group($"user_{userId}").SendAsync(method, payload);
        }

        public Task SendToGroupAsync(string groupName, string method, object payload)
        {
            return _hub.Clients.Group(groupName).SendAsync(method, payload);
        }
    }
}


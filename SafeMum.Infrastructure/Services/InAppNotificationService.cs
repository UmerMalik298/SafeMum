using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.AppNotification;

using Supabase.Postgrest;
using System.Text.Json;

namespace SafeMum.Infrastructure.Services
{
    public class InAppNotificationService : IInAppNotificationService
    {
        private readonly Supabase.Client _client;
        private readonly INotificationGateway _gateway;
        private readonly ILogger<InAppNotificationService> _logger;

        public InAppNotificationService(
            ISupabaseClientFactory clientFactory,
            INotificationGateway gateway,
            ILogger<InAppNotificationService> logger)
        {
            _client = clientFactory.GetClient();
            _gateway = gateway;
            _logger = logger;
        }

        public async Task<Guid> CreateNotificationAsync(Guid userId, string title, string message, string type = "general", object data = null)
        {
            try
            {
                var notification = new InAppNotification
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Title = title,
                    Message = message,
                    Type = type,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    Data = data != null ? JsonSerializer.Serialize(data) : null
                };

                await _client.From<InAppNotification>().Insert(notification);

                await _gateway.SendToUserAsync(userId, "NewNotification", new
                {
                    notification.Id,
                    notification.Title,
                    notification.Message,
                    notification.Type,
                    notification.CreatedAt,
                    notification.IsRead,
                    Data = data,

                });

                _logger.LogInformation("In-app notification created for user {UserId}: {Title}", userId, title);
                return notification.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating in-app notification");
                throw;
            }
        }

        public async Task<List<InAppNotification>> GetUserNotificationsAsync(Guid userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var res = await _client
                    .From<InAppNotification>()
                    .Filter("userid", Constants.Operator.Equals, userId.ToString())
                    .Order("createdat", Constants.Ordering.Descending)
                    .Range((page - 1) * pageSize, page * pageSize - 1)
                    .Get();

                return res.Models;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notifications for user {UserId}", userId);
                return new List<InAppNotification>();
            }
        }

        public async Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId)
        {
            try
            {
                var res = await _client
                    .From<InAppNotification>()
                    .Filter("id", Constants.Operator.Equals, notificationId.ToString())
                    .Filter("userid", Constants.Operator.Equals, userId.ToString())   // match your actual column name
                    .Set(x => x.IsRead, true)                                          // <-- lambda
                    .Set(x => x.ReadAt, DateTime.UtcNow)                               // <-- lambda
                    .Update();

                if (res.Models.Any())
                {
                    await _gateway.SendToUserAsync(userId, "NotificationRead", notificationId);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification as read");
                return false;
            }
        }

        public async Task<bool> MarkAllAsReadAsync(Guid userId)
        {
            try
            {
                await _client
                    .From<InAppNotification>()
                    .Filter("userid", Constants.Operator.Equals, userId.ToString())    // match your actual column name
                    .Filter("isread", Constants.Operator.Equals, "false")
                    .Set(x => x.IsRead, true)                                          // <-- lambda
                    .Set(x => x.ReadAt, DateTime.UtcNow)                               // <-- lambda
                    .Update();

                await _gateway.SendToUserAsync(userId, "AllNotificationsRead", new { });
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking all notifications as read");
                return false;
            }
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            try
            {
                var res = await _client
                    .From<InAppNotification>()
                    .Filter("userid", Constants.Operator.Equals, userId.ToString())
                    .Filter("isread", Constants.Operator.Equals, "false")
                    .Get();

                return res.Models.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread count");
                return 0;
            }
        }

        public async Task<bool> DeleteNotificationAsync(Guid notificationId, Guid userId)
        {
            try
            {
                await _client
                    .From<InAppNotification>()
                    .Filter("id", Constants.Operator.Equals, notificationId.ToString())
                    .Filter("userid", Constants.Operator.Equals, userId.ToString())
                    .Delete();

                await _gateway.SendToUserAsync(userId, "NotificationDeleted", notificationId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting notification");
                return false;
            }
        }
    }
}
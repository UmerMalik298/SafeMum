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

namespace SafeMum.Application.Services
{

   
        public class InAppNotificationService : IInAppNotificationService
        {
            private readonly Supabase.Client _client;
            private readonly IHubContext<NotificationHub> _hubContext;
            private readonly ILogger<InAppNotificationService> _logger;

            public InAppNotificationService(
                ISupabaseClientFactory clientFactory,
                IHubContext<NotificationHub> hubContext,
                ILogger<InAppNotificationService> logger)
            {
                _client = clientFactory.GetClient();
                _hubContext = hubContext;
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

                    // Send real-time notification via SignalR
                    await _hubContext.Clients.User(userId.ToString()).SendAsync("NewNotification", new
                    {
                        notification.Id,
                        notification.Title,
                        notification.Message,
                        notification.Type,
                        notification.CreatedAt,
                        notification.IsRead,
                        Data = data
                    });

                    _logger.LogInformation($"In-app notification created for user {userId}: {title}");
                    return notification.Id;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating in-app notification: {ex.Message}");
                    throw;
                }
            }

            public async Task<List<InAppNotification>> GetUserNotificationsAsync(Guid userId, int page = 1, int pageSize = 20)
            {
                try
                {
                    var notifications = await _client
                        .From<InAppNotification>()
                        .Filter("userid", Supabase.Postgrest.Constants.Operator.Equals, userId.ToString())
                        .Order("created_at", Supabase.Postgrest.Constants.Ordering.Descending)
                        .Range((page - 1) * pageSize, page * pageSize - 1)
                        .Get();

                    return notifications.Models;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error getting notifications for user {userId}: {ex.Message}");
                    return new List<InAppNotification>();
                }
            }

            public async Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId)
            {
                try
                {
                    var notification = new InAppNotification
                    {
                        Id = notificationId,
                        IsRead = true,
                        ReadAt = DateTime.UtcNow
                    };

                    var result = await _client
                        .From<InAppNotification>()
                        .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, notificationId.ToString())
                        .Filter("userid", Supabase.Postgrest.Constants.Operator.Equals, userId.ToString())
                        .Update(notification);

                    if (result.Models.Any())
                    {
                        // Notify client about read status change
                        await _hubContext.Clients.User(userId.ToString()).SendAsync("NotificationRead", notificationId);
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error marking notification as read: {ex.Message}");
                    return false;
                }
            }

            public async Task<bool> MarkAllAsReadAsync(Guid userId)
            {
                try
                {
                    var update = new Dictionary<string, object>
                    {
                        ["is_read"] = true,
                        ["read_at"] = DateTime.UtcNow
                    };

                    await _client
                        .From<InAppNotification>()
                        .Filter("userid", Supabase.Postgrest.Constants.Operator.Equals, userId.ToString())
                        .Filter("is_read", Supabase.Postgrest.Constants.Operator.Equals, "false")
                        .Update(update);

                    // Notify client about all read
                    await _hubContext.Clients.User(userId.ToString()).SendAsync("AllNotificationsRead");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error marking all notifications as read: {ex.Message}");
                    return false;
                }
            }

            public async Task<int> GetUnreadCountAsync(Guid userId)
            {
                try
                {
                    var notifications = await _client
                        .From<InAppNotification>()
                        .Filter("userid", Supabase.Postgrest.Constants.Operator.Equals, userId.ToString())
                        .Filter("is_read", Supabase.Postgrest.Constants.Operator.Equals, "false")
                        .Get();

                    return notifications.Models.Count;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error getting unread count: {ex.Message}");
                    return 0;
                }
            }

            public async Task<bool> DeleteNotificationAsync(Guid notificationId, Guid userId)
            {
                try
                {
                    await _client
                        .From<InAppNotification>()
                        .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, notificationId.ToString())
                        .Filter("userid", Supabase.Postgrest.Constants.Operator.Equals, userId.ToString())
                        .Delete();

                    // Notify client about deletion
                    await _hubContext.Clients.User(userId.ToString()).SendAsync("NotificationDeleted", notificationId);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error deleting notification: {ex.Message}");
                    return false;
                }
            }
        }
    }
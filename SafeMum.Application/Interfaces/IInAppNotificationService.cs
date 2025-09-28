using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.AppNotification;

namespace SafeMum.Application.Interfaces
{
    public interface IInAppNotificationService
    {
        Task<Guid> CreateNotificationAsync(Guid userId, string title, string message, string type = "general", object data = null);
        Task<List<InAppNotification>> GetUserNotificationsAsync(Guid userId, int page = 1, int pageSize = 20);
        Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId);
        Task<bool> MarkAllAsReadAsync(Guid userId);
        Task<int> GetUnreadCountAsync(Guid userId);
        Task<bool> DeleteNotificationAsync(Guid notificationId, Guid userId);
    }
}

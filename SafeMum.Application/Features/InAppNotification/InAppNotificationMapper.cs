using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.InAppNotification
{
    public static class InAppNotificationMapper
    {
        public static InAppNotificationDto ToDto(
            this SafeMum.Domain.Entities.AppNotification.InAppNotification n) =>
            new InAppNotificationDto(
                n.Id, n.UserId, n.Title, n.Message, n.Type,
                n.IsRead, n.CreatedAt, n.ReadAt, n.Data
            );
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.InAppNotification
{
    public record InAppNotificationDto(
         Guid Id,
         Guid UserId,
         string Title,
         string Message,
         string Type,
         bool IsRead,
         DateTime CreatedAt,
         DateTime? ReadAt,
         string? Data
     );
}

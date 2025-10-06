using MediatR;
using SafeMum.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.InAppNotification.DeleteNotification
{
    public class DeleteNotificationRequest : IRequest<Result>
    {
        public Guid NotificationId { get; init; }
    
    }
}

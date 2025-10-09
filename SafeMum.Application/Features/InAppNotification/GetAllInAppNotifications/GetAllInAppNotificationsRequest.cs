using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.InAppNotification.GetAllInAppNotifications
{
    public class GetAllInAppNotificationsRequest : IRequest<List<GetAllInAppNotificationsResponse>>
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }

    public class GetAllInAppNotificationsResponse
    {
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public Guid Id { get; set; }
    }
}

using MediatR;
using SafeMum.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.InAppNotification.GetUnreadCount
{
    public class GetUnreadCountRequest : IRequest<Result<int>>
    {
    }
}

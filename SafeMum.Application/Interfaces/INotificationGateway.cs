using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Interfaces
{
    public interface INotificationGateway
    {
        Task SendToUserAsync(Guid userId, string method, object payload);
        Task SendToGroupAsync(string groupName, string method, object payload);
    }
}

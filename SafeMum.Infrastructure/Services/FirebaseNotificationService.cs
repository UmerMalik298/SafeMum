using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Application.Interfaces;

namespace SafeMum.Infrastructure.Services
{
    public class FirebaseNotificationService : IPushNotificationService
    {


        public Task SendPushNotification(string deviceToken, string title, string body)
        {
            throw new NotImplementedException();
        }
    }
}

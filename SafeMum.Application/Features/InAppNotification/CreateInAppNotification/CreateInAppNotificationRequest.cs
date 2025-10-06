using MediatR;
using SafeMum.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.InAppNotification.CreateInAppNotification
{
    public class CreateInAppNotificationRequest : IRequest<Result>
    {
        public Guid? UserId { get; set; }               
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string Type { get; set; } = "general";   
        public object? Data { get; set; }                
    }
}

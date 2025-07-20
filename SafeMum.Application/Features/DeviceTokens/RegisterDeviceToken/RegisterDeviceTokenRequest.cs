using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.DeviceTokens.RegisterDeviceToken
{
    public class RegisterDeviceTokenRequest : IRequest<Result>
    {
        public Guid UserId { get; set; }
        public string DeviceToken { get; set; }
    }
}

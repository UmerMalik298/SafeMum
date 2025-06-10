using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Users.ResetPassword
{
    public class ResetPasswordRequest : IRequest<ResetPasswordResponse>
    {
        public string NewPassword { get; set; }

      
    }

    public class ResetPasswordResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

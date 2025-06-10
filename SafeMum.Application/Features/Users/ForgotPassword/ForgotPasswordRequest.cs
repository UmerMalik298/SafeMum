using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Users.ForgotPassword
{
    public class ForgotPasswordRequest : IRequest<ForgotPasswordResponse>
    {

       
   
        public string Email { get; set; }
    }

    public class ForgotPasswordResponse
    {
      public  bool Success { get; set; }
       public string Message { get; set; }


        
    }
}

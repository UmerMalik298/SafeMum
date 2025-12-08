using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.Users.ResetPasswordRedirect
{
    public class ResetPasswordRedirectRequest : IRequest<ResetPasswordRedirectResponse>
    {
      
    }

    public class ResetPasswordRedirectResponse
    {
        public string HtmlContent { get; set; }
    }
}

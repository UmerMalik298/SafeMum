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
        public string TokenHash { get; set; }  // Changed from AccessToken
        public string Type { get; set; }
    }

    public class ResetPasswordRedirectResponse
    {
        public string HtmlContent { get; set; }
    }
}

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
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiresAt { get; set; }
        public string TokenType { get; set; }
    }

    public class ResetPasswordRedirectResponse
    {
        public string HtmlContent { get; set; }
    }
}

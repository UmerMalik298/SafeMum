using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;

namespace SafeMum.Application.Features.Users.Logout
{
    public class LogoutUserHandler : IRequestHandler<LogoutUserRequest, Result>
    {
        private readonly Supabase.Client _client;
        public LogoutUserHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();

        }
        public async Task<Result> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                //var session = _client.Auth.CurrentSession;
                //var user = _client.Auth.CurrentUser;

                await _client.Auth.SignOut();

                if (_client.Auth.CurrentSession == null)
                    return Result.Success();
                else
                    return Result.Failure("Logout failed, session still exists.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Logout Failed: {ex.Message}");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;

namespace SafeMum.Application.Features.Users.ForgotPassword
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordRequest, ForgotPasswordResponse>
    {
        private readonly Supabase.Client _client;
        public ForgotPasswordHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
            
        }
        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _client.Auth.ResetPasswordForEmail(request.Email);

                return new ForgotPasswordResponse
                {
                    Success = true,
                    Message = "Password Reset Email has been sent"
                };
            }
            catch (Exception ex)
            {

                return new ForgotPasswordResponse
                {
                    Success = false,
                    Message = "Faild to send reset Email"+ ex.ToString()
                };
            }
          
        }
    }
}

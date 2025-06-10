using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;

namespace SafeMum.Application.Features.Users.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest, ResetPasswordResponse>
    {
        private readonly Supabase.Client _client;

        public ResetPasswordHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
            
        }
        public async Task<ResetPasswordResponse> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {

            try
            {
                await _client.Auth.Update(new Supabase.Gotrue.UserAttributes
                {
                    Password = request.NewPassword,
                });


                return new ResetPasswordResponse
                {
                    Success = true,
                    Message = "Password has been reset Successfuly"
                };


            }
            catch (Exception ex)
            {

                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = $"Reset failed: {ex.Message}"
                };
            }
            
        }
    }
}

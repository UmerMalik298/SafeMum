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
                // Validate that we have the access token
                if (string.IsNullOrEmpty(request.AccessToken))
                {
                    return new ResetPasswordResponse
                    {
                        Success = false,
                        Message = "Access token is required"
                    };
                }

                // Set the session using the tokens from the reset email
                await _client.Auth.SetSession(request.AccessToken, request.RefreshToken);

                // Now update the password
                await _client.Auth.Update(new Supabase.Gotrue.UserAttributes
                {
                    Password = request.NewPassword,
                });

                return new ResetPasswordResponse
                {
                    Success = true,
                    Message = "Password has been reset successfully"
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

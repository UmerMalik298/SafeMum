using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Users;

namespace SafeMum.Application.Features.Users.ResetPassword
{
   

    public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest, ResetPasswordResponse>
    {
        private readonly Supabase.Client _client;
        private readonly ISupabaseAdminService _adminService;

        public ResetPasswordHandler(
            ISupabaseClientFactory clientFactory,
            ISupabaseAdminService adminService)
        {
            _client = clientFactory.GetClient();
            _adminService = adminService;
        }

        public async Task<ResetPasswordResponse> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Validate token
                var tokenResponse = await _client
                    .From<PasswordResetToken>()
                    .Where(x => x.Token == request.AccessToken)
                    .Where(x => x.Used == false)
                    .Where(x => x.ExpiresAt > DateTime.UtcNow)
                    .Single();

                if (tokenResponse == null)
                {
                    return new ResetPasswordResponse
                    {
                        Success = false,
                        Message = "Invalid or expired reset token"
                    };
                }

                // 2. Get user by email using admin service
                var user = await _adminService.GetUserByEmailAsync(tokenResponse.Email);

                if (user == null)
                {
                    return new ResetPasswordResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // 3. Update password using admin service (service role key)
                var passwordUpdated = await _adminService.UpdateUserPasswordAsync(user.Id, request.NewPassword);

                if (!passwordUpdated)
                {
                    return new ResetPasswordResponse
                    {
                        Success = false,
                        Message = "Failed to update password"
                    };
                }

                // 4. Mark token as used
                await _client
                    .From<PasswordResetToken>()
                    .Where(x => x.Token == request.AccessToken)
                    .Set(x => x.Used, true)
                    .Update();

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

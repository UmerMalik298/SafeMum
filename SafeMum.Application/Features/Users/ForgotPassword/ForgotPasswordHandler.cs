using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Users;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.Users.ForgotPassword
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordRequest, ForgotPasswordResponse>
    {
        private readonly Supabase.Client _client;
        private readonly ISupabaseAdminService _adminService;

        public ForgotPasswordHandler(
            ISupabaseClientFactory clientFactory,
            ISupabaseAdminService adminService)
        {
            _client = clientFactory.GetClient();
            _adminService = adminService;
        }

        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Check if user exists
                var user = await _adminService.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    return new ForgotPasswordResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // 2. Generate token
                var resetToken = GenerateSecureToken();
                var expiresAt = DateTime.UtcNow.AddHours(1);

                // 3. Insert using DTO (no Id property)
                var tokenInsert = new PasswordResetToken
                {
                    Email = request.Email,
                    Token = resetToken,
                    ExpiresAt = expiresAt,
                    Used = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _client.From<PasswordResetToken>().Insert(tokenInsert);

                return new ForgotPasswordResponse
                {
                    Success = true,
                    Message = $"Password reset token: {resetToken}"
                };
            }
            catch (Exception ex)
            {
                return new ForgotPasswordResponse
                {
                    Success = false,
                    Message = $"Failed to send reset email: {ex.Message}"
                };
            }
        }

        private string GenerateSecureToken()
        {
            var randomBytes = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
    }
}
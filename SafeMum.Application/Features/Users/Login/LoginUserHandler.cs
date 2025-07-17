using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Users;
using Supabase.Gotrue.Exceptions;

namespace SafeMum.Application.Features.Users.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserRequest, LoginUserResponse>
    {
        private readonly Supabase.Client _client;
        public LoginUserHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
            
        }
        public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _client.Auth.SignInWithPassword(request.Email, request.Password);

                var  userId = result.User.Id;
                var userid = Guid.Parse(userId);


                var roleResult = await _client.From<User>().Where(x => x.Id == userid).Single();


                return new LoginUserResponse
                {
                    UserId = result.User.Id,
                    Success = true,
                    Message = "Login successful",

                    Email = result.User.Email,
                    Token = result.AccessToken,
                    RefreshToken = result.RefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddSeconds(result.ExpiresIn),
                    Role = roleResult.Role

                };
            }
            catch (GotrueException ex)
            {
                return new LoginUserResponse
                {
                    Success = false,
                    Message = ex.Message.Contains("invalid_credentials")
                        ? "Invalid email or password"
                        : ex.Message
                };
            }
            catch (Exception ex)
            {
                return new LoginUserResponse
                {
                    Success = false,
                    Message = $"Login failed: {ex.Message}"
                };
            }
        }
    }
}

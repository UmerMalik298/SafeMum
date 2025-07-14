using MediatR;
using SafeMum.Application.Features.Users.CreateUser;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Users;
using Supabase;

namespace SafeMum.Application.Features.Users.CreateUser
{
    public class CreateUserHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly Supabase.Client _client;

        public CreateUserHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
        }

        public async Task<RegisterUserResponse> Handle(
            RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                // 1. Create auth user
                var authResponse = await _client.Auth.SignUp(request.Email, request.Password);

                if (authResponse?.User?.Id == null)
                    return FailedResponse("Authentication failed");

                // 2. Create public user
                var user = new User
                {
                    Id = Guid.Parse(authResponse.User.Id),
                    Email = request.Email,
                    Username = request.Username,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CreatedAt = DateTime.UtcNow,
                    Role = request.Role ?? "User",
                    UserType = request.UserType?? "Visitor"
                };

                await _client.From<User>().Insert(user);

                return new RegisterUserResponse
                {
                    Success = true,
                    UserId = authResponse.User.Id,
                    Email = user.Email,
                    Username = user.Username,
                    CreatedAt = user.CreatedAt,
                    UserType = user.UserType
                };
            }
            catch (Exception ex)
            {
               
                if (ex.Message.Contains("already registered") || ex.Message.Contains("already in use"))
                    return FailedResponse("Email already registered");

                return FailedResponse($"Registration failed: {ex.Message}");
            }
        }

        private RegisterUserResponse FailedResponse(string message) => new()
        {
            Success = false,
            ErrorMessage = message
        };
    }
}
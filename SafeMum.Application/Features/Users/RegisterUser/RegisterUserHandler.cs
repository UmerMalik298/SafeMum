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
            // 1. Check if email exists in auth system
            var authUser = await _client.Auth.SignIn(request.Email);
            if (authUser?.User?.Id != null)
            {
                return FailedResponse("Email already registered");
            }

            // 2. Create auth user
            var authResponse = await _client.Auth.SignUp(request.Email, request.Password);

            if (authResponse?.User?.Id == null)
                return FailedResponse("Authentication failed");

            // 3. Create public user with SAME ID
            var user = new User
            {
                Id = Guid.Parse(authResponse.User.Id), // Critical: Same ID
                Email = request.Email,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow,
                Role = "User",
                UserType = request.UserType // Set user type
            };

            await _client.From<User>().Insert(user);

            return new RegisterUserResponse
            {
                Success = true,
                UserId = authResponse.User.Id,
                Email = user.Email,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                UserType = user.UserType // Return user type
            };
        }

        private RegisterUserResponse FailedResponse(string message) =>
            new RegisterUserResponse
            {
                Success = false,
                ErrorMessage = message
            };
    }
}
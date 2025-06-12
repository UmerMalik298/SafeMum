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
            var alreadyUser = await _client.From<User>().Where(e => e.Email == request.Email).Single();
            if(alreadyUser != null)
            {
                return FailedResponse("Email Already Exists try another Email");
            }
           
            var authResponse = await _client.Auth.SignUp(request.Email, request.Password);

            if (authResponse?.User?.Id == null)
                return FailedResponse("Authentication failed");

        
            var user = new User
            {
                Id = Guid.Parse(authResponse.User.Id),
                Email = request.Email,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow,
                Role = "User"
            };

            await _client.From<User>().Insert(user);

          
            return new RegisterUserResponse
            {
                Success = true,
                UserId = authResponse.User.Id,
                Email = user.Email,
                Username = user.Username,
                CreatedAt = user.CreatedAt
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
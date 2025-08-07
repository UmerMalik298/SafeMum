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
        private readonly ISupabaseAdminService _adminService;

        public CreateUserHandler(ISupabaseClientFactory clientFactory, ISupabaseAdminService adminService)
        {
            _client = clientFactory.GetClient();
            _adminService = adminService;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
         
                var authResponse = await _client.Auth.SignUp(request.Email, request.Password);

                if (authResponse?.User?.Id == null)
                    return FailedResponse("Authentication failed");

                var userId = Guid.Parse(authResponse.User.Id);

                var user = new User
                {
                    Id = userId,
                    Email = request.Email,
                    Username = request.Username,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CreatedAt = DateTime.UtcNow,
                    Role = request.Role ?? "User",
                    UserType = request.UserType ?? "Visitor"
                };

                try
                {
                    await _client.From<User>().Insert(user);
                }
                catch (Exception dbEx)
                {
                   
                    await _adminService.DeleteUserAsync(authResponse.User.Id);
                    return FailedResponse($"User creation failed: {dbEx.Message}");
                }

                // Success
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
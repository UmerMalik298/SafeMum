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
                // Step 1: Auth signup
                var authResponse = await _client.Auth.SignUp(request.Email, request.Password);

                if (authResponse?.User?.Id == null)
                    return FailedResponse("Authentication failed");

                Guid userId = Guid.Parse(authResponse.User.Id);

                // Step 2: Create User model
                var user = new User
                {
                    Id = userId,
                    Email = request.Email,
                    Username = request.Username,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Role = request.Role ?? "User",
                    UserType = request.UserType ?? "Visitor",
                    DeviceToken = null,
                    ProfileUrl = null,
                    PhoneNo = null,
                    Address = null
                };

                // Step 3: Insert using model-based Insert
                try
                {
                    await _client.From<User>().Insert(user);
                }
                catch (Exception dbEx)
                {
                    // rollback auth user
                    await _adminService.DeleteUserAsync(authResponse.User.Id);
                    return FailedResponse($"User creation failed: {dbEx.Message}");
                }

                // Step 4: Return success response
                return new RegisterUserResponse
                {
                    Success = true,
                    UserId = authResponse.User.Id,
                    Email = user.Email,
                    Username = user.Username,
                    CreatedAt = DateTime.UtcNow,
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
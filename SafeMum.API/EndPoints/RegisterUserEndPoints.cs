using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeMum.Application.Features.Users.CreateUser;
using SafeMum.Application.Features.Users.ForgotPassword;
using SafeMum.Application.Features.Users.Login;
using SafeMum.Application.Features.Users.Logout;
using SafeMum.Application.Features.Users.ResetPassword;
using SafeMum.Application.Features.Users.ResetPasswordRedirect;
using SafeMum.Application.Features.Users.UpdateProfile;

namespace SafeMum.API.EndPoints
{
    public static class RegisterUserEndPoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/users").WithTags("RegisterUser");
            

            group.MapPost("/register", async (RegisterUserRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });
            group.MapPost("/login", async (LoginUserRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });
            group.MapPost("/logout", async (LogoutUserRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            }).RequireAuthorization();

            group.MapPost("/forgot-password", async (ForgotPasswordRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapGet("/reset-password-redirect", async ([AsParameters] ResetPasswordRedirectRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapPut("/reset-password", async (ResetPasswordRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });
            group.MapPatch("/update-profile", async ([FromForm] UpdateProfileRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            }).DisableAntiforgery();





            return app;
        }
    }

}

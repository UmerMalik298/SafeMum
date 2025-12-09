using MediatR;
using Microsoft.AspNetCore.Http;
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

            //app.MapGet("/api/users/reset-password-redirect", async (IMediator mediator) =>
            //{
            //    var result = await mediator.Send(new ResetPasswordRedirectRequest());
            //    return Results.Content(result.HtmlContent, "text/html");
            //});

            app.MapGet("/api/users/reset-password-redirect", async (
    IMediator mediator,
    HttpContext context) =>
            {
                // Extract parameters from query string
                var accessToken = context.Request.Query["access_token"].ToString();
                var refreshToken = context.Request.Query["refresh_token"].ToString();
                var expiresAt = context.Request.Query["expires_at"].ToString();
                var tokenType = context.Request.Query["token_type"].ToString();

                var result = await mediator.Send(new ResetPasswordRedirectRequest
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt,
                    TokenType = tokenType
                });

                return Results.Content(result.HtmlContent, "text/html");
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

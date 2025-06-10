using MediatR;

using SafeMum.Application.Features.Users.CreateUser;
using SafeMum.Application.Features.Users.ForgotPassword;
using SafeMum.Application.Features.Users.Login;
using SafeMum.Application.Features.Users.ResetPassword;

namespace SafeMum.API.EndPoints
{
    public static class RegisterUserEndPoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/users");

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

            group.MapPost("/forgot-password", async (ForgotPasswordRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapPut("/reset-password", async (ResetPasswordRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });




            return app;
        }
    }

}

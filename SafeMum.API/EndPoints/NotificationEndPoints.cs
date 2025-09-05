using MediatR;
using SafeMum.Application.Features.Content.CreateContentGroup;
using SafeMum.Application.Features.DeviceTokens.RegisterDeviceToken;

namespace SafeMum.API.EndPoints
{
    public static class NotificationEndPoints
    {
        public static IEndpointRouteBuilder MapNotificationEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/notification").WithTags("Notification");
            group.RequireAuthorization();

            group.MapPost("/register-device-token", async (RegisterDeviceTokenRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            return app;
        }

    }
}

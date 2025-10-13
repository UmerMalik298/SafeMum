using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Features.Communication.GetMessagesByUser;
using SafeMum.Application.Features.Content.CreateContentGroup;
using SafeMum.Application.Features.DeviceTokens.RegisterDeviceToken;
using SafeMum.Application.Features.DeviceTokens.TestNotification;
using SafeMum.Application.Features.InAppNotification.CreateInAppNotification;
using SafeMum.Application.Features.InAppNotification.DeleteNotification;
using SafeMum.Application.Features.InAppNotification.GetAllInAppNotifications;
using SafeMum.Application.Features.InAppNotification.GetUnreadCount;
using SafeMum.Application.Features.InAppNotification.MarkAllRead;
using SafeMum.Application.Features.InAppNotification.MarkNotificationRead;

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
            group.MapPost("/test-notification", async (TestNotificationRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });


            group.MapPost("/", async (CreateInAppNotificationRequest req, ISender sender) =>
            {
                var result = await sender.Send(req);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            // List (paged)
            group.MapGet("/", async (int page, int pageSize, ISender sender) =>
            {
                var list = await sender.Send(new GetAllInAppNotificationsRequest { Page = page, PageSize = pageSize });
                return Results.Ok(list);   // ✅ no .IsSuccess on a List<>
            });
            // Mark one read
            group.MapPatch("/{id:guid}/read", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new MarkNotificationReadRequest { NotificationId = id });
                return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
            });

            // Mark all read
            group.MapPatch("/read-all", async (ISender sender) =>
            {
                var result = await sender.Send(new MarkAllReadRequest());
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            // Unread count
       
            group.MapGet("/unread-count", async ([AsParameters] GetUnreadCountRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            // Delete
            group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteNotificationRequest { NotificationId = id });
                return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
            });

            return app;
        }

    }
}

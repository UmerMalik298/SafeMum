using MediatR;
using SafeMum.Application.Features.Communication.ChatGroups.AddUsers;
using SafeMum.Application.Features.Communication.ChatGroups.CreateChatGroup;
using SafeMum.Application.Features.Communication.ChatGroups.GetAllChatGroup;
using SafeMum.Application.Features.Communication.ChatGroups.GetGroupMessages;
using SafeMum.Application.Features.Communication.ChatGroups.GetUsersGroup;
using SafeMum.Application.Features.Communication.GetAllConversation;
using SafeMum.Application.Features.Communication.GetAllMessages;
using SafeMum.Application.Features.Communication.GetAllUsers;
using SafeMum.Application.Features.Communication.GetMessagesByUser;
using SafeMum.Application.Features.Communication.GetUserById;
using SafeMum.Application.Features.PregnancyTracker.CreateWeeklyProfile;
using SafeMum.Application.Features.PregnancyTracker.GetWeeklyProfile;

namespace SafeMum.API.EndPoints
{
    public static class CommunicationEndPoints
    {
        public static IEndpointRouteBuilder MapCommunicationEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/communication").WithTags("Communication");
            group.RequireAuthorization();

           

            group.MapGet("/get-all-users-messages", async ([AsParameters] GetAllMessagesRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapGet("/get-message-by-user-request", async ([AsParameters] GetMessagesByUserRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });




            group.MapGet("/get-all-user", async ([AsParameters] GetAllUsersRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });



            group.MapGet("/get-user-by-id", async ([AsParameters] GetUserByIdRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });
            group.MapGet("/get-conversation-by-userid", async ([AsParameters] GetAllConversationRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });




            group.MapPost("/create-chat-group", async ( CreateChatGroupRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });




            group.MapPost("/add-user-in-chat-group", async (AddUserRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapGet("/get-all-group", async ([AsParameters] GetAllChatGroupRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });



            group.MapGet("/get-all-user-groups", async ([AsParameters] GetUsersGroupRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapGet("/get-group-messages", async ([AsParameters] GetGroupMessagesRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            return app;
        }
    }
}

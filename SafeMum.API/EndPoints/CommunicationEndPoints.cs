using MediatR;
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

            return app;
        }
    }
}

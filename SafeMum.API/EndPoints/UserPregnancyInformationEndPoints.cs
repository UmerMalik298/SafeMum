using MediatR;
using SafeMum.Application.Features.PregnancyTracker.CreateWeeklyProfile;
using SafeMum.Application.Features.UserPregnancyInformation;

namespace SafeMum.API.EndPoints
{
    public static class UserPregnancyInformationEndPoints
    {
        public static IEndpointRouteBuilder MapUserPregnancyInformationEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/user-pregnancy-information").WithTags("UserPregnancyInformation");
            group.RequireAuthorization();

            group.MapPost("/create-user-pregnancy-information", async (UserPregnancyInformationRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);


            });

            return app;
        }
    }
}

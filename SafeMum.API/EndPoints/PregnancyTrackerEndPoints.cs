using System.Reflection.Metadata.Ecma335;
using MediatR;
using SafeMum.Application.Features.Content.CreateContentGroup;
using SafeMum.Application.Features.PregnancyTracker.CreateWeeklyProfile;
using SafeMum.Application.Features.PregnancyTracker.GetWeeklyProfile;

namespace SafeMum.API.EndPoints
{
    public static class PregnancyTrackerEndPoints
    {
        public static IEndpointRouteBuilder MapPregnancyTrackerEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/pregnancy-tracker").WithTags("PregnancyTracker");
            group.RequireAuthorization();

            group.MapPost("/create-pregnancy-tracker", async (CreateWeeklyProfileRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);


            });

            group.MapGet("/weekly-pregnancy-profile", async ([AsParameters] GetWeeklyProfileRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            return app;
        }
    }
        
}

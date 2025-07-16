using MediatR;
using SafeMum.Application.Features.NutritionHealthTracking.PrenatalAppointments.AddPrenatalAppointment;
using SafeMum.Application.Features.NutritionHealthTracking.Supplement.AddSupplement;
using SafeMum.Application.Features.NutritionHealthTracking.WaterIntake.AddWaterIntake;
using SafeMum.Application.Features.PregnancyTracker.CreateWeeklyProfile;
using SafeMum.Application.Features.PregnancyTracker.GetWeeklyProfile;

namespace SafeMum.API.EndPoints
{
    public static class NutritionTrackingEndPoints
    {
        public static IEndpointRouteBuilder MapNutritionTrackingEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/nutrition-tracker").WithTags("Nutrition Health Tracking");
            group.RequireAuthorization();

            group.MapPost("/add-water-intake-log", async (AddWaterIntakeRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);


            });



            group.MapPost("/add-supplement-intake-log", async (AddSupplementRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);


            });



            group.MapPost("/add-prenatal-appointment", async (AddPrenatalAppointmentRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);


            });
            //group.MapGet("/weekly-pregnancy-profile", async ([AsParameters] GetWeeklyProfileRequest request, IMediator mediator) =>
            //{
            //    var result = await mediator.Send(request);
            //    return Results.Ok(result);
            //});

            return app;
        }
    }
}


using MediatR;
using SafeMum.Application.Features.Content.CreateContentGroup;
using SafeMum.Application.Features.Users.CreateUser;
using SafeMum.Application.Features.Users.Login;

namespace SafeMum.API.EndPoints
{
    public static class ContentEndPoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/content").WithTags("Content");

            group.MapPost("/create-content-group", async (CreateContentGroupRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });
          




            return app;
        }
    }
}

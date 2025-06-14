using MediatR;

using SafeMum.Application.Features.Content.CreateContentGroup;
using SafeMum.Application.Features.Content.CreateContentItem;
using SafeMum.Application.Features.Content.DeleteContentItem;
using SafeMum.Application.Features.Content.GetAllContentGroup;
using SafeMum.Application.Features.Content.GetAllContentItem;
using SafeMum.Application.Features.Content.GetContentItemById;
using SafeMum.Application.Features.Users.CreateUser;
using SafeMum.Application.Features.Users.Login;

namespace SafeMum.API.EndPoints
{
    public static class ContentEndPoints
    {
        public static IEndpointRouteBuilder MapContentEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/content").WithTags("Content");
            group.RequireAuthorization();

            group.MapPost("/create-content-group", async (CreateContentGroupRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });
            group.MapDelete("/delete-content-item", async ([AsParameters] DeleteContentItemRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            //group.MapPost("/create-content-item", async ([AsParameters] CreateContentItemRequest request, IMediator mediator) =>
            //{
            //    var result = await mediator.Send(request);
            //    return Results.Ok(result);
            //});


            group.MapPost("/create-content-item", async (
    HttpRequest httpRequest,
    IFormFile image,
    IMediator mediator) =>
            {
                var form = httpRequest.Form;

                var request = new CreateContentItemRequest
                {
                    TitleEn = form["titleEn"],
                    
                    SummaryEn = form["summaryEn"],
                 
                    TextEn = form["textEn"],
                   
                    Category = form["category"],
                    Audience = form["audience"],
                    Tags = form["tags"].ToString().Split(',').Select(tag => tag.Trim()).ToList(),

                    Image = image
                };

                var result = await mediator.Send(request);
                return Results.Ok(result);
            }).DisableAntiforgery();



            group.MapGet("/get-all-content-item", async ([AsParameters] GetAllContentItemRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });


            group.MapGet("/get-all-content-group", async ([AsParameters] GetAllContentGroupRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapGet("/content-item/{id}", async ([AsParameters] Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetContentItemByIdRequest { Id = id });
                return Results.Ok(result);
            });


            return app;
        }
    }
}

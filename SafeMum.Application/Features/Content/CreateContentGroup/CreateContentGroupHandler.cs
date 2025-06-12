//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.WebSockets;
//using System.Text;
//using System.Threading.Tasks;
//using MediatR;
//using SafeMum.Application.Common;
//using SafeMum.Application.Interfaces;
//using SafeMum.Domain.Entities.Content;


//namespace SafeMum.Application.Features.Content.CreateContentGroup
//{
//    public class CreateContentGroupHandler : IRequestHandler<CreateContentGroupRequest, Result>
//    {
//        private readonly Supabase.Client _client;
//        public CreateContentGroupHandler(ISupabaseClientFactory clientFactory)
//        {
//            _client = clientFactory.GetClient();

//        }

//        public async Task<Result> Handle(CreateContentGroupRequest request, CancellationToken cancellationToken)
//        {


//            var contentGroup = new ContentGroup
//            {
//                Title = request.Title,
//                Description = request.Description,
//              //  ContentItemIds = request.ContentItemIds.Select(g => g.ToString()).ToList()

//            };

            
//                await _client.From<ContentGroup>().Insert(contentGroup);



//                return Result.Success();
            
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Users;

namespace SafeMum.Application.Features.Communication.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        private readonly Supabase.Client _client;
        public GetUserByIdHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
            
        }
        public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            var userId = await _client.From<User>().Where(x => x.Id == request.Id).Get();

            if (userId == null)
            {

                throw new ApplicationException("User Not Found");

            }


            return new GetUserByIdResponse {


                Name = userId.Model.FirstName,
                Email = userId.Model.Email,

            }; 

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Common.Pagination;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Users;

using Supabase.Interfaces;

namespace SafeMum.Application.Features.Communication.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersRequest, PaginatedResponse<GetAllUserResponse>>
    {
        private readonly Supabase.Client _client;
        public GetAllUsersHandler(ISupabaseClientFactory supabaseClient)
        {
            
            _client = supabaseClient.GetClient();
        }
        public async Task<PaginatedResponse<GetAllUserResponse>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
        {
          
            var users = await _client
         .From<User>() 
         .Get();


            var result = users.Models.Select(u => new GetAllUserResponse
            {
                Id = u.Id,
                Name = $"{u.FirstName} {u.LastName}".Trim(),
                Email = u.Email,
               // PhoneNumber = u.PhoneNumber
            }).ToList();

            return new PaginatedResponse<GetAllUserResponse>
            {
                Data = result,
               
            };
        }

    }
}

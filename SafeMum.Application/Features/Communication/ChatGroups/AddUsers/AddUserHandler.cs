using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Common.Exceptions;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Communication;

namespace SafeMum.Application.Features.Communication.ChatGroups.AddUsers
{
    public class AddUserHandler : IRequestHandler<AddUserRequest, IResult>
    {
        private readonly Supabase.Client _client;
        public AddUserHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
            
        }
        public async Task<IResult> Handle(AddUserRequest request, CancellationToken cancellationToken)
        {
            var groupId = await _client.From<ChatGroup>().Where(x => x.Id == request.GroupId).Single();

            if (groupId == null)
            {

                throw new AppException("Group Not Found");
            }

            var result = new GroupMember
            {
                Id = new Guid(),
                ChatGroupId = request.GroupId,
                UserId = request.UserId,
            };

            await _client.From<GroupMember>().Insert(result);


            return Result.Success();
            
        }
    }
}

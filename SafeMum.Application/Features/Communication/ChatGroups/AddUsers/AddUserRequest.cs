using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.Communication.ChatGroups.AddUsers
{
    public class AddUserRequest : IRequest<IResult>
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common.Pagination;

namespace SafeMum.Application.Features.Communication.GetAllUsers
{
    public class GetAllUsersRequest : IRequest<PaginatedResponse<GetAllUserResponse>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }


    public class GetAllUserResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}

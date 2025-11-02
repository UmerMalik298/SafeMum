using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Communication.GetUserById
{
    public class GetUserByIdRequest : IRequest<GetUserByIdResponse>
    {
        public Guid Id { get; set; }
    }


    public class GetUserByIdResponse
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string ProfileUrl { get; set; }
    }
}

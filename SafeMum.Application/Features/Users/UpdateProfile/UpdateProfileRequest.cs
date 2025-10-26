using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.Users.UpdateProfile
{
    public class UpdateProfileRequest : IRequest<Result>
    {
        public Guid UserId { get; set; }
        public string? PhoneNo { get; set; }

        public string? Address { get; set; }

        public IFormFile? ProfileImage { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.Content.CreateContentGroup
{
    public class CreateContentGroupRequest : IRequest<Result>
    {
        public string Title { get; set; }
        public string? Description { get; set; }

      //  public List<Guid> ContentItemIds { get; set; }
    }
}

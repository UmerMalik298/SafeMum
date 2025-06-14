using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.Content.DeleteContentItem
{
    public class DeleteContentItemRequest : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}

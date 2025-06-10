using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Content.DeleteContentItem
{
    public class DeleteContentItemRequest : IRequest
    {
        public Guid Id { get; set; }
    }
}

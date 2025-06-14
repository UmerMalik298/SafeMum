using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Content.GetContentGroupById
{
    public class GetContentGroupByIdRequest : IRequest<GetContentGroupByIdResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetContentGroupByIdResponse
    {
        public string Title { get; set; }

        public string Description { get; set; }
         public string Auidence { get; set; }
        public string Category { get; set; }

        

    }
}

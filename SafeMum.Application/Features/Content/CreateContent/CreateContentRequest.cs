using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Content.CreateContent
{
    public class CreateContentRequest : IRequest<CreateContentResponse>
    {
        public Guid GroupId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public List<string>? Tags { get; set; }
    }
    public class CreateContentResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

    }


}

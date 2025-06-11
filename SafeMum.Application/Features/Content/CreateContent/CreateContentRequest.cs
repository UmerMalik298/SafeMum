using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.Content.CreateContent
{
    public class CreateContentRequest : IRequest<Result>
    {
     
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }


        public string? Category { get; set; }
        public string? Audience { get; set; }
        public List<string>? Tags { get; set; }
    }
    


}

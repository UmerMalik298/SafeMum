using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.Content.CreateContentItem
{
    public class CreateContentItemRequest : IRequest<Result>
    {
        public string TitleEn { get; set; }
        public string SummaryEn { get; set; }
        public string TextEn { get; set; }

        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public string Audience { get; set; }

        public List<string> Tags { get; set; }
    }

}

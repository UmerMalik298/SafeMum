using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Content.GetAllContentItem
{
    public class GetAllContentItemRequest : IRequest<List<GetAllContentItemResponse>>
    {
        public string? Category { get; set; }
        public string? Audience { get; set; }

        public string? Language { get; set; } = "en";
    };


    public class GetAllContentItemResponse
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }

        public string Audience { get; set; }


        public string Summary { get; set; }

        public List<string> Tags { get; set; }

    }
}

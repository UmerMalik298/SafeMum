using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Content.GetAllContentGroupItems
{
    public class GetAllContentGroupItemsRequest : IRequest<List<ContentGroupDto>>
    {
        public string? Language { get; set; } = "en";
    }



    public class ContentItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Audience { get; set; }
        public string Summary { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Tags { get; set; }
    }

    public class ContentGroupDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Audience { get; set; }
        public List<ContentItemDto> Items { get; set; }
    }

}

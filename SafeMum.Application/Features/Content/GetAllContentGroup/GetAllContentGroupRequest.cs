using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Content.GetAllContentGroup
{
    public class GetAllContentGroupRequest : IRequest<List<GetAllContentGroupsResponse>>
    {
        public string? Category { get; set; }
        public string? Audience { get; set; }
        public string? Language { get; set; } = "en";
    }
    public class GetAllContentGroupsResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Audience { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Tags { get; set; }
        public List<Guid> ContentItemIds { get; set; }
    }

}

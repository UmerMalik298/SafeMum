using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Content.GetContentItemById
{
    public class GetContentItemByIdRequest : IRequest<GetContentItemByIdResponse>
    {
        public Guid Id { get; set; }
    }
    public class GetContentItemByIdResponse
    {
        public string Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleUr { get; set; }
        public string SummaryEn { get; set; }
        public string SummaryUr { get; set; }
        public string TextEn { get; set; }
        public string TextUr { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public string Audience { get; set; }
        public List<string> Tags { get; set; }
    }




}

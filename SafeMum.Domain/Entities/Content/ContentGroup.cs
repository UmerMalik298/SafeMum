using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeMum.Domain.Entities.Content
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
   
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    namespace SafeMum.Domain.Entities.Content
    {
        [Table("content_groups")]
        public class ContentGroup : BaseEntity
        {
            [Column("title")]
            public string Title { get; set; }

            [Column("description")]
            public string? Description { get; set; }

            [Column("content_item_ids")]
            [JsonPropertyName("content_item_ids")]
            public List<string> ContentItemIds { get; set; } = new();
        }
    }

}

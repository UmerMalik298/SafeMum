using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.Content
 {
        [Table("content_groups")]
        public class ContentGroup : BaseModel
        {
            
            


            [Column("title")]
            public string Title { get; set; }

            [Column("description")]
            public string? Description { get; set; }

            [Column("content_item_ids")]
 
            public List<string> ContentItemIds { get; set; } = new();
        }
    }



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace SafeMum.Domain.Entities.Content
 {
    [System.ComponentModel.DataAnnotations.Schema.Table("content_groups")]
    public class ContentGroup : BaseModel
        {


        [PrimaryKey("id", false)]
        public Guid Id { get; set; }


        [Supabase.Postgrest.Attributes.Column("title")]
            public string Title { get; set; }

            [Supabase.Postgrest.Attributes.Column("description")]
            public string? Description { get; set; }

            //[Supabase.Postgrest.Attributes.Column("content_item_ids")]
 
            //public List<string> ContentItemIds { get; set; } = new();
        }
    }



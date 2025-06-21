using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.Content
{
   
        [Table("contentgroups")]
        public class contentGroups : BaseModel
        {
            [PrimaryKey("id", false)]
            public Guid Id { get; set; }

            [Column("title")]
            public string title { get; set; }

            [Column("description")]
            public string description { get; set; }

            [Column("audience")]
            public string audience { get; set; }

            [Column("category")]
            public string category { get; set; }

            [Column("language")]
            public string language { get; set; }

        

            [Column("tags")]
            public List<string> tags { get; set; }

        [Column("contentItemIds")]
        public List<Guid> contentItemIds { get; set; }

    }
    }


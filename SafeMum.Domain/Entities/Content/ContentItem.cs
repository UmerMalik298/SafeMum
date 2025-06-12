using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.Content
{
    [Table("contentitem")]
    public class contentitem : BaseModel
    {

        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("title_en")]
        public string title_en { get; set; }

        [Column("title_ur")]
        public string title_ur { get; set; }


        [Column("summary_en")]
        public string summary_en { get; set; }

        [Column("summary_ur")]
        public string summary_ur { get; set; }


        [Column("text_en")]
        public string text_en { get; set; }


        [Column("text_ur")]
        public string text_ur { get; set; }

        [Column("image_url")]
        public string image_url { get; set; }



        [Column("category")]
        public string category { get; set; }


        [Column("audience")]
        public string audience { get; set; }


        [Column("tags")]
        public List<string> tags { get; set; }

        
    }
}

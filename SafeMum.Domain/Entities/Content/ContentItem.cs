using SafeMum.Domain.Entities.Common;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeMum.Domain.Entities.Content
{
    [System.ComponentModel.DataAnnotations.Schema.Table("content_items")]
    public class ContentItem : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; } 

        [System.ComponentModel.DataAnnotations.Schema.Column("title_en")]
        public string TitleEn { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("title_ur")]
        public string TitleUr { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("summary_en")]
        public string SummaryEn { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("summary_ur")]
        public string SummaryUr { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("text_en")]
        public string TextEn { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("text_ur")]
        public string TextUr { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("image_url")]
        public string ImageUrl { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("category")]
        public string Category { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("audience")]
        public string Audience { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("tags")]
        public List<string> Tags { get; set; } // JSON string or list
    }

}

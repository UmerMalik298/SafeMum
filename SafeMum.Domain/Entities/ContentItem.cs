using SafeMum.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeMum.Domain.Entities.Content
{
    [Table("content_items")]
    public class ContentItem : BaseEntity
    {
        [Column("group_id")]
        public Guid GroupId { get; set; } 

        [Column("title")]
        public string Title { get; set; }

        [Column("summary")]
        public string Summary { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Column("language")]
        public string Language { get; set; } = "en"; // 'en' or 'ur'

        [Column("audience")]
        public string? Audience { get; set; }

        [Column("category")]
        public string? Category { get; set; }

        [Column("tags")]
        public string[]? Tags { get; set; } // PostgreSQL Array (Supabase supports this)
    }
}

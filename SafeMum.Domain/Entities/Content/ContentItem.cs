using SafeMum.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeMum.Domain.Entities.Content
{
    [Table("content_items")]
    public class ContentItem : BaseEntity
    {
        [Column("title_en")]
        public string TitleEn { get; set; }

        [Column("title_ur")]
        public string TitleUr { get; set; }

        [Column("summary_en")]
        public string? SummaryEn { get; set; }

        [Column("summary_ur")]
        public string? SummaryUr { get; set; }

        [Column("text_en")]
        public string TextEn { get; set; }

        [Column("text_ur")]
        public string TextUr { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("category")]
        public string? Category { get; set; }

        [Column("audience")]
        public string? Audience { get; set; }

        [Column("tags")]
        public string? Tags { get; set; } // JSON string or list
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeMum.Domain.Entities.Content
{
    [Table("content_groups")]
    public class ContentGroup : BaseEntity
    {
        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("audience")]
        public string? Audience { get; set; } // e.g. "New Mothers", "Pregnant Women"

        [Column("category")]
        public string? Category { get; set; } // e.g. "Mental Health", "Nutrition"

        [Column("language")]
        public string Language { get; set; } = "en"; // 'en' or 'ur'
    }
}

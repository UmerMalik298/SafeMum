using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.Common;
using Supabase.Postgrest.Attributes;

namespace SafeMum.Domain.Entities.AppNotification
{
    [Table("inappnotification")]    
    public class InAppNotification : BaseEntity
    {
        [Column("userid")]
        public Guid UserId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("type")]
        public string Type { get; set; } // appointment, reminder, general

        [Column("isread")]
        public bool IsRead { get; set; }

        [Column("data")]
        public string Data { get; set; } // JSON string

        [Column("readat")]
        public DateTime? ReadAt { get; set; }
    }
}

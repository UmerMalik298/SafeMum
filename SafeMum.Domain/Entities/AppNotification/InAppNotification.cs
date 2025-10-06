using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.Common;
using Supabase.Postgrest.Attributes;

namespace SafeMum.Domain.Entities.AppNotification
{
    public class InAppNotification : BaseEntity
    {

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("type")]
        public string Type { get; set; } // appointment, reminder, general

        [Column("is_read")]
        public bool IsRead { get; set; }

        [Column("data")]
        public string Data { get; set; } // JSON string

        [Column("read_at")]
        public DateTime? ReadAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}

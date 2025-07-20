using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.Common
{
    [Table("devicetoken")]
    public class DeviceToken : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("userid")]
        public Guid UserId { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }


}

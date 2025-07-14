using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.Communication
{
    [Table("ChatGroup")]
    public class ChatGroup : BaseModel
    {
        [Column("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();


        [Column("Name")]
        public string Name { get; set; }


        [Column("AdminUserId")]
        public Guid AdminUserId { get; set; }


        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [JsonIgnore]

        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    }
}

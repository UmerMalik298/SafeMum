using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.Communication
{
    [Table("GroupMember")]
    public class GroupMember : BaseModel
    {

        [Column("Id")]
        public Guid Id { get; set; }

        [Column("ChatGroupId")]
        public Guid ChatGroupId { get; set; }


        [Column("UserId")]
        public Guid UserId { get; set; }


    }
}

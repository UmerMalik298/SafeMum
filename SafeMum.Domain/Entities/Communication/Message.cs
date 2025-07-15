using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SafeMum.Domain.Entities.Communication
{


    [Supabase.Postgrest.Attributes.Table("message")]
    public class Message : BaseModel
    {

        [PrimaryKey("id", false)]
        public Guid Id { get; set; }


        [Supabase.Postgrest.Attributes.Column("senderid")]
        public Guid SenderId { get; set; }


        [Supabase.Postgrest.Attributes.Column("receiverid")]
        public Guid ReceiverId { get; set; }

        [Supabase.Postgrest.Attributes.Column("content")]
        public string Content { get; set; }


        [Supabase.Postgrest.Attributes.Column("sendat")]
        public DateTime SendAt { get; set; }


        [Supabase.Postgrest.Attributes.Column("issent")]
        public bool? IsSent { get; set; }

        [Supabase.Postgrest.Attributes.Column("isseen")]
        public bool? IsSeen { get; set; }


        [Supabase.Postgrest.Attributes.Column("groupid")]
        public Guid? GroupId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.Common;
using Supabase.Postgrest.Attributes;

namespace SafeMum.Domain.Entities.Users
{
    [Table("users")]
    public class User : BaseEntity
    {
        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("username")]
        public string Username { get; set; } = null!;

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("role")]
        public string Role { get; set; } = "User";

        [Column("is_email_verified")]
        public bool IsEmailVerified { get; set; } = false;

        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }


        [Column("user_type")]
        public string UserType { get; set; }
    }
}
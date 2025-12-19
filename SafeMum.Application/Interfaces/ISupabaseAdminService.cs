using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Interfaces
{
    public interface ISupabaseAdminService
    {
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> UpdateUserPasswordAsync(string userId, string newPassword);
        Task<SupabaseUser> GetUserByEmailAsync(string email);
    }
    public class SupabaseUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }
}

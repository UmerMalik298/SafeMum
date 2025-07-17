using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Users.Login
{
    public class LoginUserRequest : IRequest<LoginUserResponse>
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public class LoginUserResponse
    {
        public string UserId { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public string RefreshToken { get; set; }


        public string Email { get; set; }
 
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public string Role { get; set; }
    }
}
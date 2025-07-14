using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SafeMum.Application.Features.Users.CreateUser
{
    public class RegisterUserRequest : IRequest<RegisterUserResponse>
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        [StringLength(100, ErrorMessage = "Email Cant exceed 100 characters")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
         ErrorMessage = "Password must contain uppercase, lowercase, number, and special character")]

        public string Password { get; set; }


        [Required(ErrorMessage = "UserName required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "UserName must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$",
            ErrorMessage = "Username can only contain letters, numbers, and underscores")]
        public string Username { get; set; }



        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }

        public string? UserType { get; set; } = "Visitor";

        public string? Role { get; set; } = "User";


    }

    public class RegisterUserResponse
    {
        public bool Success { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ErrorMessage { get; set; }

        public string UserType { get; set; }

        public string Role { get; set; }
        public Dictionary<string, string> ValidationErrors { get; set; } = new();

    }

}

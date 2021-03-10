using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWeb.DTOs
{
    public class RegisterDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [MaxLength(32)]
        public string Username { get; set; }
        [Required]
        [MaxLength(40)]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public string Role { get; set; } = "user";
    }
}

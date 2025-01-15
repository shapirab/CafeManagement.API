using CafeManagement.Data.DataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.DataModels.DTOs
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public Role UserRole { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}

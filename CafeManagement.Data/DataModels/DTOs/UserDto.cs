using CafeManagement.Data.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.DataModels.DTOs
{
    public class UserDto
    {
        public string? Username { get; set; }
        public string? Token { get; set; }
    }
}

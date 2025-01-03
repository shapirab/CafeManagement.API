﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagement.Data.DataModels.DTOs
{
    public class RegisterUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
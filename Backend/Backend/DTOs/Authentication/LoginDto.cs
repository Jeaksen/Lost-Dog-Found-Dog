using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs.Authentication
{
    public class LoginDto
    {
        [Required(ErrorMessage = "User Name is required")]
        [MaxLength(256)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8)]
        public string Password { get; set; }
    }
}

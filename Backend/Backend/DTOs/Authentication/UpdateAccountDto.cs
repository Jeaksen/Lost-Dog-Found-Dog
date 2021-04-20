using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs.Authentication
{
    public class UpdateAccountDto
    {

        [Required(ErrorMessage = "User Name is required")]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(20)]
        [MinLength(8)]
        public string PhoneNumber { get; set; }
    }
}


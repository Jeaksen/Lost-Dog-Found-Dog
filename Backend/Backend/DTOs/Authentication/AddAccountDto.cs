using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs.Authentication
{
    public class AddAccountDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
    }
}

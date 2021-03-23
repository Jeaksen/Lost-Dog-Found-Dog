using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.Authentication
{
    public class AuthenticationResult
    {
        public string UserType { get; set; }
        public string Token { get; set; }
    }
}

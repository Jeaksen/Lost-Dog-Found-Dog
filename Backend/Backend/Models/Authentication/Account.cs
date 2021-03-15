using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.Authentication
{
    // IdentityUser contains a phone number
    public class Account : IdentityUser<int>
    {

    }
}

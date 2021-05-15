using Backend.Models.Shelters;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.Authentication
{
    // IdentityUser contains a phone number
    public class Account : IdentityUser<int>
    {
        [Required]
        [MaxLength(20)]
        [Column(TypeName ="varchar(20)")]
        public override string PhoneNumber { get; set; }

        [Required]
        public override string Email { get; set; }

        public int? ShelterId { get; set; }

        public Shelter Shelter { get; set; }
    }
}

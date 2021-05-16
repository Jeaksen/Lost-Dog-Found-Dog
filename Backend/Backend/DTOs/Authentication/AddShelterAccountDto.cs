using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs.Authentication
{
    public class AddShelterAccountDto : AddAccountDto
    {
        public int ShelterId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs.Dogs
{
    public class UploadShelterDogDto : UploadDogDto
    {
        public int ShelterId { get; set; }
    }
}

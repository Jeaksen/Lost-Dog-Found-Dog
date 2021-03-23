using AutoMapper;
using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddAccountDto, Account>();
        }
    }
}

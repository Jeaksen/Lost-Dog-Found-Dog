using AutoMapper;
using Backend.DTOs.Authentication;
using Backend.DTOs.Dogs;
using Backend.Models.Authentication;
using Backend.Models.DogBase;
using Backend.Models.DogBase.LostDog;
using System.Linq;

namespace Backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddAccountDto, Account>();
            CreateMap<AddLostDogDto, LostDog>().AfterMap((source, dest) => dest.Behaviors = source.Behaviors.Select(name => new DogBehavior() { Behvaior = name }).ToList());
            CreateMap<AddLocationDto, Location>();
            CreateMap<AddLostDogCommentDto, LostDogComment>();
        }
    }
}

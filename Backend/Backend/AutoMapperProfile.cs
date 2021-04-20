using AutoMapper;
using Backend.DataAccess;
using Backend.DTOs.Authentication;
using Backend.DTOs.Dogs;
using Backend.Models.Authentication;
using Backend.Models.DogBase;
using Backend.Models.DogBase.LostDog;
using Backend.Services;
using System.Collections.Generic;
using System.Linq;

namespace Backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddAccountDto, Account>().ForMember(a => a.UserName, opt => opt.MapFrom(dto => dto.Name));
            CreateMap<GetAccountDto, Account>().ForMember(a => a.UserName, opt => opt.MapFrom(dto => dto.Name));
            CreateMap<Account, GetAccountDto>().ForMember(dto => dto.Name, opt => opt.MapFrom(a => a.UserName));
            CreateMap<AddLostDogDto, LostDog>().ForMember(dog => dog.Behaviors, opt => opt.MapFrom(dto => dto.Behaviors.Select(s => new DogBehavior() { Behvaior = s })));
            CreateMap<AddLocationDto, Location>();
            CreateMap<AddLostDogCommentDto, LostDogComment>();
            CreateMap<UpdateLostDogDto, LostDog>().ForMember(dog => dog.Behaviors, opt => opt.MapFrom(dto => dto.Behaviors.Select(s => new DogBehavior() { Behvaior = s })));
            CreateMap(typeof(RepositoryResponse<>), typeof(ServiceResponse<>)).ForMember("StatusCode", s => s.Ignore());
        }
    }
}

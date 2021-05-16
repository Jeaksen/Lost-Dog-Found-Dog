using AutoMapper;
using Backend.DTOs.Authentication;
using Backend.DTOs.Dogs;
using Backend.DTOs.Shelters;
using Backend.Models.Authentication;
using Backend.Models.Dogs;
using Backend.Models.Dogs.LostDogs;
using Backend.Models.Response;
using Backend.Models.Shelters;
using System.Linq;

namespace Backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddAccountDto, Account>().ForMember(a => a.UserName, opt => opt.MapFrom(dto => dto.Name));
            CreateMap<AddShelterAccountDto, Account>().ForMember(a => a.UserName, opt => opt.MapFrom(dto => dto.Name));
            CreateMap<GetAccountDto, Account>().ForMember(a => a.UserName, opt => opt.MapFrom(dto => dto.Name));
            CreateMap<Account, GetAccountDto>().ForMember(dto => dto.Name, opt => opt.MapFrom(a => a.UserName));

            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();
            CreateMap<Shelter, ShelterDto>();
            CreateMap<ShelterDto, Shelter>();

            CreateMap<LocationDto, Location>();
            CreateMap<Location, LocationDto>();
            CreateMap<AddLostDogCommentDto, LostDogComment>();
            CreateMap<LostDog, GetLostDogDto>().ForMember(dto => dto.Behaviors, opt => opt.MapFrom(dto => dto.Behaviors.Select(b => b.Behavior)));
            CreateMap<UploadLostDogDto, LostDog>().ForMember(dog => dog.Behaviors, opt => opt.MapFrom(dto => dto.Behaviors.Select(s => new DogBehavior() { Behavior = s })));

            CreateMap(typeof(RepositoryResponse), typeof(ServiceResponse)).ForMember("StatusCode", s => s.Ignore());
            CreateMap(typeof(RepositoryResponse), typeof(ServiceResponse<>)).ForMember("StatusCode", s => s.Ignore()).ForMember("Data", s => s.Ignore());
            CreateMap(typeof(RepositoryResponse), typeof(ServiceResponse<,>)).ForMember("StatusCode", s => s.Ignore()).ForMember("Data", s => s.Ignore()).ForMember("Metadata", s => s.Ignore());
            CreateMap(typeof(RepositoryResponse<>), typeof(ServiceResponse)).ForMember("StatusCode", s => s.Ignore());
            CreateMap(typeof(RepositoryResponse<>), typeof(ServiceResponse<>)).ForMember("StatusCode", s => s.Ignore());
            CreateMap(typeof(RepositoryResponse<>), typeof(ServiceResponse<,>)).ForMember("StatusCode", s => s.Ignore()).ForMember("Metadata", s => s.Ignore());
            CreateMap(typeof(RepositoryResponse<,>), typeof(ServiceResponse)).ForMember("StatusCode", s => s.Ignore());
            CreateMap(typeof(RepositoryResponse<,>), typeof(ServiceResponse<>)).ForMember("StatusCode", s => s.Ignore());
            CreateMap(typeof(RepositoryResponse<,>), typeof(ServiceResponse<,>)).ForMember("StatusCode", s => s.Ignore());

            CreateMap(typeof(ServiceResponse), typeof(ControllerResponse));
            CreateMap(typeof(ServiceResponse), typeof(ControllerResponse<>)).ForMember("Data", s => s.Ignore());
            CreateMap(typeof(ServiceResponse), typeof(ControllerResponse<,>)).ForMember("Data", s => s.Ignore()).ForMember("Metadata", s => s.Ignore());
            CreateMap(typeof(ServiceResponse<>), typeof(ControllerResponse));
            CreateMap(typeof(ServiceResponse<>), typeof(ControllerResponse<>));
            CreateMap(typeof(ServiceResponse<>), typeof(ControllerResponse<,>)).ForMember("Metadata", s => s.Ignore());
            CreateMap(typeof(ServiceResponse<,>), typeof(ControllerResponse));
            CreateMap(typeof(ServiceResponse<,>), typeof(ControllerResponse<>));
            CreateMap(typeof(ServiceResponse<,>), typeof(ControllerResponse<,>));
        }
    }
}

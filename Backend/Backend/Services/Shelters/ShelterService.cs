using AutoMapper;
using Backend.DataAccess.ShelterDogs;
using Backend.DataAccess.Shelters;
using Backend.DTOs.Authentication;
using Backend.DTOs.Dogs;
using Backend.DTOs.Shelters;
using Backend.Models.Dogs;
using Backend.Models.Dogs.ShelterDogs;
using Backend.Models.Response;
using Backend.Models.Shelters;
using Backend.Services.Authentication;
using Backend.Services.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Services.Shelters
{
    public class ShelterService : IShelterService
    {
        private readonly IShelterRepository shelterRepository;
        private readonly IShelterDogRepository shelterDogRepository;
        private readonly IAccountService accountService;
        private readonly ISecurityService securityService;
        private readonly IMapper mapper;
        private readonly ILogger<ShelterService> logger;

        public ShelterService(IShelterRepository shelterRepository, IShelterDogRepository shelterDogRepository, IAccountService accountService, ISecurityService securityService, IMapper mapper, ILogger<ShelterService> logger)
        {
            this.shelterRepository = shelterRepository;
            this.shelterDogRepository = shelterDogRepository;
            this.accountService = accountService;
            this.securityService = securityService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ServiceResponse<ShelterDto, GetAccountDto>> AddShelter(ShelterDto shelterDto)
        {
            var shelter = mapper.Map<Shelter>(shelterDto);
            shelter.IsApproved = true;
            var addShelterResult = await shelterRepository.AddShelter(shelter);
            var serviceResponse = mapper.Map<RepositoryResponse<Shelter>, ServiceResponse<ShelterDto, GetAccountDto>>(addShelterResult);
            if (addShelterResult.Successful)
            {
                var addAccountResult = await accountService.AddShelterAccount(addShelterResult.Data);   
                if (addAccountResult.Successful)
                    serviceResponse.Metadata = addAccountResult.Data;
                else
                {
                    serviceResponse.Data = null;
                    serviceResponse.Successful = false;
                    var deleteResult = await shelterRepository.DeleteShelter(addShelterResult.Data.Id);
                    if (deleteResult.Successful)
                        serviceResponse.Message = $"Failed to add shelter acccount: {addAccountResult.Message}";
                    else
                        serviceResponse.Message = $"Data corrupted! Failed to add shelter account and remove shelter after failing to add acccount: {addAccountResult.Message} {deleteResult.Message}";
                }
            }
            else
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ShelterDto>, int>> GetShelters(string name, string sort, int page, int size)
        {
            var repoResponse = await shelterRepository.GetShelters(name, sort, page, size);
            var serviceResponse = mapper.Map<ServiceResponse<List<ShelterDto>, int>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse<ShelterDto>> GetShelter(int id)
        {
            var repoResponse = await shelterRepository.GetShelter(id);
            var serviceResponse = mapper.Map<RepositoryResponse<Shelter>, ServiceResponse<ShelterDto>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteShelter(int id)
        {
            var getResponse = await GetShelter(id);
            var serviceResponse = mapper.Map<ServiceResponse<ShelterDto>, ServiceResponse>(getResponse);
            if (getResponse.Successful)
            {
                var accountResponse = await accountService.DeleteAccount(email: getResponse.Data.Email);
                var shelterResponse = await shelterRepository.DeleteShelter(id);
                
                if (!accountResponse.Successful  || !shelterResponse.Successful)
                {
                    serviceResponse.Message = "Failed to delete shelter! ";
                    serviceResponse.Message += accountResponse.Successful ? "" : accountResponse.Message;
                    serviceResponse.Message += shelterResponse.Successful ? "" : shelterResponse.Message;
                    serviceResponse.Successful = false;
                    serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                }
                else
                    serviceResponse.Message = $"Shelter with id {id} deleted";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetShelterDogDto>> AddShelterDog(UploadShelterDogDto shelterDogDto, IFormFile picture)
        {
            var serviceResponse = new ServiceResponse<GetShelterDogDto>();
            var shelterDog = mapper.Map<ShelterDog>(shelterDogDto);
            byte[] data;

            if (picture is null)
            {
                serviceResponse.Successful = false;
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                serviceResponse.Message = "No picture was provided!";
            }
            else
            {
                var pictureValidationResult = securityService.IsPictureValid(picture);

                if (pictureValidationResult.Successful)
                {
                    using (var ms = new MemoryStream())
                    {
                        picture.CopyTo(ms);
                        data = ms.ToArray();
                    }
                    shelterDog.Picture = new PictureDog()
                    {
                        FileName = picture.FileName,
                        FileType = picture.ContentType,
                        Data = data
                    };
                    var repoResponse = await shelterDogRepository.AddShelterDog(shelterDog);
                    serviceResponse = mapper.Map<ServiceResponse<GetShelterDogDto>>(repoResponse);
                    if (!serviceResponse.Successful)
                        serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                }
                else
                {
                    serviceResponse.Successful = false;
                    serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                    serviceResponse.Message = pictureValidationResult.Message;
                }
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetShelterDogDto>, int>> GetShelterDogs(int shelterId, int page, int size)
        {
            var repoResponse = await shelterDogRepository.GetShelterDogs(shelterId, page, size);
            var serviceResponse = mapper.Map<ServiceResponse<List<GetShelterDogDto>, int>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetShelterDogDto>> GetShelterDogDetails(int dogId)
        {
            var repoResponse = await shelterDogRepository.GetShelterDogDetails(dogId);
            var serviceResponse = mapper.Map<ServiceResponse<GetShelterDogDto>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteShelterDog(int dogId)
        {
            var repoResponse = await shelterDogRepository.DeleteShelterDog(dogId);
            var serviceResponse = mapper.Map<ServiceResponse>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }
    }
}

using AutoMapper;
using Backend.DataAccess.Shelters;
using Backend.DTOs.Authentication;
using Backend.DTOs.Shelters;
using Backend.Models.Response;
using Backend.Models.Shelters;
using Backend.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services.Shelters
{
    public class ShelterService : IShelterService
    {
        private readonly IShelterRepository shelterRepository;
        private readonly IAccountService accountService;
        private readonly IMapper mapper;
        private readonly ILogger<ShelterService> logger;

        public ShelterService(IShelterRepository shelterRepository, IAccountService accountService, IMapper mapper, ILogger<ShelterService> logger)
        {
            this.shelterRepository = shelterRepository;
            this.accountService = accountService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ServiceResponse<ShelterDto, GetAccountDto>> AddShelter(ShelterDto shelterDto)
        {
            var shelter = mapper.Map<Shelter>(shelterDto);

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
                    var deleteResult = await shelterRepository.DeleteShelterWithoutDogs(addShelterResult.Data.Id);
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
            // Add deleting dogs
            var getResponse = await GetShelter(id);
            var serviceResponse = mapper.Map<ServiceResponse<ShelterDto>, ServiceResponse>(getResponse);
            if (getResponse.Successful)
            {
                var accountResponse = await accountService.DeleteAccount(email: getResponse.Data.Email);
                var shelterResponse = await shelterRepository.DeleteShelterWithoutDogs(id);

                if (!accountResponse.Successful || !shelterResponse.Successful)
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
    }
}

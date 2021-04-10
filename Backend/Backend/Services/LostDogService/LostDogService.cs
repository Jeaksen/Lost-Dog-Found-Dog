using AutoMapper;
using Backend.DataAccess;
using Backend.DataAccess.Dogs;
using Backend.DTOs.Dogs;
using Backend.Models.DogBase;
using Backend.Models.DogBase.LostDog;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.LostDogService
{
    public class LostDogService : ILostDogService
    {
        private readonly ILostDogRepository lostDogDataRepository;
        private readonly IMapper mapper;
        private readonly ILogger<LostDogService> logger;

        public LostDogService(ILostDogRepository lostDogDataRepository, IMapper mapper, ILogger<LostDogService> logger)
        {
            this.lostDogDataRepository = lostDogDataRepository;
            this.mapper = mapper;
            this.logger = logger;
        }


        public async Task<ServiceResponse<List<LostDog>>> GetLostDogs()
        {
            var repoResponse =  await lostDogDataRepository.GetLostDogs();
            var serviceResponse = mapper.Map<RepositoryResponse<List<LostDog>>, ServiceResponse<List<LostDog>>>(repoResponse); 
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<LostDog>>> GetUserLostDogs(int ownerId)
        {
            var repoResponse = await lostDogDataRepository.GetUserLostDogs(ownerId);
            var serviceResponse = mapper.Map<RepositoryResponse<List<LostDog>>, ServiceResponse<List<LostDog>>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
            return serviceResponse;
        }

        public async Task<ServiceResponse<LostDog>> GetLostDogDetails(int dogId)
        {
            var repoResponse = await lostDogDataRepository.GetLostDogDetails(dogId);
            var serviceResponse = mapper.Map<RepositoryResponse<LostDog>, ServiceResponse<LostDog>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
            return serviceResponse;
        }

        public async Task<ServiceResponse<LostDog>> AddLostDog(AddLostDogDto lostDogDto, IFormFile picture)
        {
            ServiceResponse<LostDog> serviceResponse = new ServiceResponse<LostDog>();
            LostDog lostDog = mapper.Map<LostDog>(lostDogDto);
            byte[] data;

            if (picture?.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    picture.CopyTo(ms);
                    data = ms.ToArray();
                }
                lostDog.Picture = new Picture()
                {
                    FileName = picture.FileName,
                    FileType = picture.ContentType,
                    Data = data
                };
                var repoResponse = await lostDogDataRepository.AddLostDog(lostDog);
                serviceResponse = mapper.Map<RepositoryResponse<LostDog>, ServiceResponse<LostDog>>(repoResponse);
                if (!serviceResponse.Successful)
                    serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
                else
                    serviceResponse.Data.Picture.Data = null;
            } 
            else
            {
                serviceResponse.Successful = false;
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                serviceResponse.Message = "No picture was provided!";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> MarkLostDogAsFound(int dogId)
        {
            var repoResponse = await lostDogDataRepository.MarkDogAsFound(dogId);
            var serviceResponse = mapper.Map<RepositoryResponse<bool>, ServiceResponse<bool>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteLostDog(int dogId)
        {
            var repoResponse = await lostDogDataRepository.DeleteLostDog(dogId);
            var serviceResponse = mapper.Map<RepositoryResponse<bool>, ServiceResponse<bool>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
            return serviceResponse;
        }

        // Image may be posted as well
        public async Task<ServiceResponse<LostDogComment>> EditLostDogComment(LostDogComment comment)
        {
            var repoResponse = await lostDogDataRepository.EditLostDogComment(comment);
            var serviceResponse = mapper.Map<RepositoryResponse<LostDogComment>, ServiceResponse<LostDogComment>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<LostDogComment>>> GetLostDogComments(int dogId)
        {
            var repoResponse = await lostDogDataRepository.GetLostDogComments(dogId);
            var serviceResponse = mapper.Map<RepositoryResponse<List<LostDogComment>>, ServiceResponse<List<LostDogComment>>> (repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
            return serviceResponse;
        }

        // Image has to be posted as well
        public async Task<ServiceResponse<LostDogComment>> AddLostDogComment(AddLostDogCommentDto commentDto)
        {
            LostDogComment comment = mapper.Map<LostDogComment>(commentDto);
            var repoResponse = await lostDogDataRepository.AddLostDogComment(comment);
            var serviceResponse = mapper.Map<RepositoryResponse<LostDogComment>, ServiceResponse<LostDogComment>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
            return serviceResponse;
        }


    }
}

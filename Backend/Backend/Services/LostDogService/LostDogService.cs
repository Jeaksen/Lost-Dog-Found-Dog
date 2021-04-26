using AutoMapper;
using Backend.DataAccess.Dogs;
using Backend.DTOs.Dogs;
using Backend.Models.DogBase;
using Backend.Models.DogBase.LostDog;
using Backend.Models.Response;
using Backend.Services.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Services.LostDogService
{
    public class LostDogService : ILostDogService
    {
        private readonly ILostDogRepository lostDogDataRepository;
        private readonly ISecurityService securityService;
        private readonly IMapper mapper;
        private readonly ILogger<LostDogService> logger;

        public LostDogService(ILostDogRepository lostDogDataRepository, ISecurityService securityService, IMapper mapper, ILogger<LostDogService> logger)
        {
            this.lostDogDataRepository = lostDogDataRepository;
            this.securityService = securityService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ServiceResponse<LostDog>> AddLostDog(AddLostDogDto lostDogDto, IFormFile picture)
        {
            var serviceResponse = new ServiceResponse<LostDog>();
            LostDog lostDog = mapper.Map<LostDog>(lostDogDto);
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
                    lostDog.Picture = new Picture()
                    {
                        FileName = picture.FileName,
                        FileType = picture.ContentType,
                        Data = data
                    };
                    var repoResponse = await lostDogDataRepository.AddLostDog(lostDog);
                    serviceResponse = mapper.Map<RepositoryResponse<LostDog>, ServiceResponse<LostDog>>(repoResponse);
                    if (!serviceResponse.Successful)
                        serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
                    else
                        serviceResponse.Data.Picture.Data = null;
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

        public async Task<ServiceResponse<List<LostDog>, int>> GetLostDogs(LostDogFilter filter, string sort, int page, int size)
        {
            var repoResponse = await lostDogDataRepository.GetLostDogs(filter, sort, page, size);
            var serviceResponse = mapper.Map<RepositoryResponse<List<LostDog>, int>, ServiceResponse<List<LostDog>, int>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse<LostDog>> GetLostDogDetails(int dogId)
        {
            var repoResponse = await lostDogDataRepository.GetLostDogDetails(dogId);
            var serviceResponse = mapper.Map<RepositoryResponse<LostDog>, ServiceResponse<LostDog>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse<LostDog>> UpdateLostDog(UpdateLostDogDto lostDogDto, IFormFile picture, int dogId)
        {
            var lostDog = mapper.Map<LostDog>(lostDogDto);
            lostDog.Id = dogId;

            if (picture is not null)
            {
                var pictureValidationResult = securityService.IsPictureValid(picture);

                if (pictureValidationResult.Successful)
                {
                    byte[] data;
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
                }
                else return new ServiceResponse<LostDog>()
                            {
                                Successful = false,
                                StatusCode = StatusCodes.Status400BadRequest,
                                Message = pictureValidationResult.Message,
                            };
            }
            
            var repoResponse = await lostDogDataRepository.UpdateLostDog(lostDog);
            var serviceResponse = mapper.Map<RepositoryResponse<LostDog>, ServiceResponse<LostDog>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;

            return serviceResponse;
        }

        public async Task<ServiceResponse> MarkLostDogAsFound(int dogId)
        {
            var repoResponse = await lostDogDataRepository.MarkDogAsFound(dogId);
            var serviceResponse = mapper.Map<RepositoryResponse, ServiceResponse>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteLostDog(int dogId)
        {
            var repoResponse = await lostDogDataRepository.DeleteLostDog(dogId);
            var serviceResponse = mapper.Map<RepositoryResponse, ServiceResponse>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }
        

        // Image has to be posted as well
        public async Task<ServiceResponse<LostDogComment>> AddLostDogComment(AddLostDogCommentDto commentDto)
        {
            LostDogComment comment = mapper.Map<LostDogComment>(commentDto);
            var repoResponse = await lostDogDataRepository.AddLostDogComment(comment);
            var serviceResponse = mapper.Map<RepositoryResponse<LostDogComment>, ServiceResponse<LostDogComment>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<LostDogComment>>> GetLostDogComments(int dogId)
        {
            var repoResponse = await lostDogDataRepository.GetLostDogComments(dogId);
            var serviceResponse = mapper.Map<RepositoryResponse<List<LostDogComment>>, ServiceResponse<List<LostDogComment>>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        // Image may be posted as well
        public async Task<ServiceResponse<LostDogComment>> EditLostDogComment(LostDogComment comment)
        {
            var repoResponse = await lostDogDataRepository.EditLostDogComment(comment);
            var serviceResponse = mapper.Map<RepositoryResponse<LostDogComment>, ServiceResponse<LostDogComment>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }
    }
}

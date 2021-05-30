using AutoMapper;
using Backend.DataAccess.LostDogs;
using Backend.DTOs.Dogs;
using Backend.Models.Dogs;
using Backend.Models.Dogs.LostDogs;
using Backend.Models.Response;
using Backend.Services.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Services.LostDogs
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

        public async Task<ServiceResponse<GetLostDogDto>> AddLostDog(UploadLostDogDto lostDogDto, IFormFile picture)
        {
            var serviceResponse = new ServiceResponse<GetLostDogDto>();
            var lostDog = mapper.Map<LostDog>(lostDogDto);
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
                    lostDog.Picture = new PictureDog()
                    {
                        FileName = picture.FileName,
                        FileType = picture.ContentType,
                        Data = data
                    };
                    var repoResponse = await lostDogDataRepository.AddLostDog(lostDog);
                    serviceResponse = mapper.Map<RepositoryResponse<LostDog>, ServiceResponse<GetLostDogDto>>(repoResponse);
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

        public async Task<ServiceResponse<List<GetLostDogDto>, int>> GetLostDogs(LostDogFilter filter, string sort, int page, int size)
        {
            var repoResponse = await lostDogDataRepository.GetLostDogs(filter, sort, page, size);
            var serviceResponse = mapper.Map<RepositoryResponse<List<LostDog>, int>, ServiceResponse<List<GetLostDogDto>, int>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetLostDogWithCommentsDto>> GetLostDogDetails(int dogId)
        {
            var repoResponse = await lostDogDataRepository.GetLostDogDetails(dogId);
            var serviceResponse = mapper.Map<ServiceResponse<GetLostDogWithCommentsDto>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetLostDogDto>> UpdateLostDog(UploadLostDogDto lostDogDto, IFormFile picture, int dogId)
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
                    lostDog.Picture = new PictureDog()
                    {
                        FileName = picture.FileName,
                        FileType = picture.ContentType,
                        Data = data
                    };
                }
                else 
                    return new ServiceResponse<GetLostDogDto>()
                            {
                                Successful = false,
                                StatusCode = StatusCodes.Status400BadRequest,
                                Message = pictureValidationResult.Message,
                            };
            }
            
            var repoResponse = await lostDogDataRepository.UpdateLostDog(lostDog);
            var serviceResponse = mapper.Map<RepositoryResponse<LostDog>, ServiceResponse<GetLostDogDto>>(repoResponse);
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
            var serviceResponse = mapper.Map<ServiceResponse>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCommentDto>> AddLostDogComment(UploadCommentDto commentDto, IFormFile picture)
        {
            var comment = mapper.Map<LostDogComment>(commentDto);

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
                    comment.Picture = new PictureComment()
                    {
                        FileName = picture.FileName,
                        FileType = picture.ContentType,
                        Data = data
                    };
                }
                else
                    return new ServiceResponse<GetCommentDto>()
                    {
                        Successful = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = pictureValidationResult.Message,
                    };
            }

            var repoResponse = await lostDogDataRepository.AddLostDogComment(comment);
            var serviceResponse = mapper.Map<ServiceResponse<GetCommentDto>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteLostDogComment(int dogId, int commentId)
        {
            var repoResponse = await lostDogDataRepository.DeleteLostDogComment(dogId, commentId);
            var serviceResponse = mapper.Map<ServiceResponse>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCommentDto>> GetLostDogComment(int dogId, int commentId)
        {
            var repoResponse = await lostDogDataRepository.GetLostDogComment(dogId, commentId);
            var serviceResponse = mapper.Map<ServiceResponse<GetCommentDto>>(repoResponse);
            if (!serviceResponse.Successful)
                serviceResponse.StatusCode = StatusCodes.Status400BadRequest;
            return serviceResponse;
        }
    }
}

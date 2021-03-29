using AutoMapper;
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
        private readonly ILostDogRepository _lostDogDataRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LostDogService> _logger;

        public LostDogService(ILostDogRepository lostDogDataRepository, IMapper mapper, ILogger<LostDogService> logger)
        {
            _lostDogDataRepository = lostDogDataRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<LostDog>> AddLostDog(AddLostDogDto lostDogDto, IFormFile image)
        {
            ServiceResponse<LostDog> serviceResponse = new ServiceResponse<LostDog>();
            LostDog lostDog = _mapper.Map<LostDog>(lostDogDto);
            byte[] data;
            if (image != null && image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    data = ms.ToArray();
                }
                lostDog.Picture = new Picture()
                {
                    FileName = image.FileName,
                    FileType = image.ContentType,
                    Data = data
                };
                serviceResponse.Data = await _lostDogDataRepository.AddLostDog(lostDog);
                if (serviceResponse.Data == null)
                {
                    serviceResponse.Successful = false;
                    serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
                    serviceResponse.Message = "Failed to Add Dog!";
                } 
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

        public async Task<ServiceResponse<LostDogComment>> AddLostDogComment(AddLostDogCommentDto commentDto)
        {
            ServiceResponse<LostDogComment> serviceResponse = new ServiceResponse<LostDogComment>();
            LostDogComment comment = _mapper.Map<LostDogComment>(commentDto);
            serviceResponse.Data = await _lostDogDataRepository.AddLostDogComment(comment);
            if (serviceResponse.Data == null)
            {
                serviceResponse.Successful = false;
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
                serviceResponse.Message = "Failed to Add Comment to the Dog!";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteLostDog(int dogId)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            serviceResponse.Data = await _lostDogDataRepository.DeleteLostDog(dogId);
            if (serviceResponse.Data == false)
            {
                serviceResponse.Successful = false;
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
                serviceResponse.Message = "Failed to Remove Dog!";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<LostDogComment>> EditLostDogComment(LostDogComment comment)
        {
            ServiceResponse<LostDogComment> serviceResponse = new ServiceResponse<LostDogComment>();
            serviceResponse.Data = await _lostDogDataRepository.EditLostDogComment(comment);
            if (serviceResponse.Data == null)
            {
                serviceResponse.Successful = false;
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
                serviceResponse.Message = "Failed to Edit Comment!";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<LostDogComment>>> GetLostDogComments(int dogId)
        {
            ServiceResponse<List<LostDogComment>> serviceResponse = new ServiceResponse<List<LostDogComment>>();
            serviceResponse.Data = await _lostDogDataRepository.GetLostDogComments(dogId);
            if (serviceResponse.Data == null)
            {
                serviceResponse.Successful = false;
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
                serviceResponse.Message = "Failed to Get List of Comments!";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<LostDog>> GetLostDogDetails(int dogId)
        {
            ServiceResponse<LostDog> serviceResponse = new ServiceResponse<LostDog>();
            serviceResponse.Data = await _lostDogDataRepository.GetLostDogDetails(dogId);
            if (serviceResponse.Data == null)
            {
                serviceResponse.Successful = false;
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
                serviceResponse.Message = "Failed to Get Dog!";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<LostDog>>> GetLostDogs()
        {
            ServiceResponse<List<LostDog>> serviceResponse = new ServiceResponse<List<LostDog>>();
            serviceResponse.Data = await _lostDogDataRepository.GetLostDogs();
            if (serviceResponse.Data == null)
            {
                serviceResponse.Successful = false;
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
                serviceResponse.Message = "Failed to Get List of Lost Dogs!";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<LostDog>>> GetUserLostDogs(int ownerId)
        {
            ServiceResponse<List<LostDog>> serviceResponse = new ServiceResponse<List<LostDog>>();
            serviceResponse.Data = await _lostDogDataRepository.GetUserLostDogs(ownerId);
            if (serviceResponse.Data == null)
            {
                serviceResponse.Successful = false;
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;
                serviceResponse.Message = "Failed to Get List of User's LostDogs!";
            }
            return serviceResponse;
        }
    }
}

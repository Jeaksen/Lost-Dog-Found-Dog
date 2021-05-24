using AutoMapper;
using Backend.DTOs.Authentication;
using Backend.DTOs.Dogs;
using Backend.DTOs.Shelters;
using Backend.Models.Authentication;
using Backend.Models.Response;
using Backend.Services.Shelters;
using Backend.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Authorize]
    [Route("/shelters/")]
    [ApiController]
    public class ShelterController : ControllerBase
    {

        private readonly IShelterService shelterService;
        private readonly IMapper mapper;

        public ShelterController(IShelterService shelterService, IMapper mapper)
        {
            this.shelterService = shelterService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetShelters([FromQuery] string name, [FromQuery] string sort,
                                                     [FromQuery] int page = 0, [FromQuery] int size = 10)
        {
            var serviceResponse = await shelterService.GetShelters(name, sort, page, size);
            var controllerResponse = mapper.Map<ControllerResponse<List<ShelterDto>, int>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [HttpGet]
        [Route("{shelterId}")]
        public async Task<IActionResult> GetShelter(int shelterId)
        {
            var serviceResponse = await shelterService.GetShelter(shelterId);
            var controllerResponse = mapper.Map<ServiceResponse<ShelterDto>, ControllerResponse<ShelterDto>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddShelter([ModelBinder(BinderType = typeof(JsonModelBinder))] ShelterDto shelter)
        {

            var serviceResponse = await shelterService.AddShelter(shelter);
            var controllerResponse = mapper.Map<ServiceResponse<ShelterDto, GetAccountDto>, ControllerResponse<ShelterDto, GetAccountDto>>(serviceResponse);
            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [HttpDelete]
        [Route("{shelterId}")]
        public async Task<IActionResult> DeleteShelter(int shelterId)
        {
            var serviceResponse = await shelterService.DeleteShelter(shelterId);
            var controllerResponse = mapper.Map<ServiceResponse, ControllerResponse>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [HttpGet]
        [Route("{shelterId}/dogs")]
        public async Task<IActionResult> GetShelterDogs(int shelterId, [FromQuery] int page = 0, [FromQuery] int size = 10)
        {
            var serviceResponse = await shelterService.GetShelterDogs(shelterId, page, size);
            var controllerResponse = mapper.Map<ControllerResponse<List<GetShelterDogDto>, int>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [HttpGet]
        [Route("{shelterId}/dogs/{dogId}")]
        public async Task<IActionResult> GetShelterDogDetails(int dogId)
        {
            var serviceResponse = await shelterService.GetShelterDogDetails(dogId);
            var controllerResponse = mapper.Map<ControllerResponse<GetShelterDogDto>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [HttpPost]
        [Authorize(Roles = AccountRoles.Shelter)]
        [Route("{shelterId}/dogs")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddShelterDog(int shelterId, [ModelBinder(BinderType = typeof(JsonModelBinder))] UploadShelterDogDto dog,
                                                    IFormFile picture)
        {
            if (picture is null)
                return BadRequest(new ControllerResponse()
                {
                    Message = "No image was provided!",
                    Successful = false
                });
            var tokenShelterId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            if (shelterId != tokenShelterId)
                return Unauthorized(new ControllerResponse()
                {
                    Message = "Cannot add dogs to other shelters!",
                    Successful = false
                });
            dog.ShelterId = tokenShelterId;

            var serviceResponse = await shelterService.AddShelterDog(dog, picture);
            var controllerResponse = mapper.Map<ControllerResponse<GetShelterDogDto>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [HttpDelete]
        [Authorize(Roles = AccountRoles.Shelter)]
        [Route("{shelterId}/dogs/{dogId}")]
        public async Task<IActionResult> DeleteShelterDog(int shelterId, int dogId)
        {
            var savedDogResponse = await shelterService.GetShelterDogDetails(dogId);
            if (savedDogResponse.Data == null)
                return StatusCode(savedDogResponse.StatusCode, mapper.Map<ControllerResponse<GetShelterDogDto>>(savedDogResponse));

            var tokenShelterId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (shelterId != int.Parse(tokenShelterId) || tokenShelterId != savedDogResponse.Data.ShelterId.ToString())
                return Unauthorized(new ControllerResponse()
                {
                    Message = "Attempted to delete a dog which is not owned by the user!",
                    Successful = false
                });
            else
            {
                var serviceResponse = await shelterService.DeleteShelterDog(dogId);
                var controllerResponse = mapper.Map<ControllerResponse>(serviceResponse);
                return StatusCode(serviceResponse.StatusCode, controllerResponse);
            }
        }

    }
}

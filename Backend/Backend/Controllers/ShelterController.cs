using AutoMapper;
using Backend.DTOs.Authentication;
using Backend.DTOs.Shelters;
using Backend.Models.Authentication;
using Backend.Models.Response;
using Backend.Services.Shelters;
using Backend.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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

        //[HttpGet]
        //[Route("{dogId}")]
        //public async Task<IActionResult> GetShelterDogDetails(int dogId)
        //{
        //    var serviceResponse = await shelterService.GetLostDogDetails(dogId);
        //    var controllerResponse = mapper.Map<ServiceResponse<GetLostDogDto>, ControllerResponse<GetLostDogDto>>(serviceResponse);

        //    return StatusCode(serviceResponse.StatusCode, controllerResponse);
        //}

    }
}

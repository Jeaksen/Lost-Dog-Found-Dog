using AutoMapper;
using Backend.DTOs.Dogs;
using Backend.Models.Authentication;
using Backend.Models.Dogs.LostDogs;
using Backend.Models.Response;
using Backend.Services.LostDogs;
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
    [Authorize(Roles = AccountRoles.Regular)]
    [Route("/lostdogs/")]
    [ApiController]
    public class LostDogController : ControllerBase
    {
        private readonly ILostDogService lostDogService;
        private readonly IMapper mapper;

        public LostDogController(ILostDogService lostDogService, IMapper mapper)
        {
            this.lostDogService = lostDogService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetLostDogs([FromQuery(Name = "filter")] LostDogFilter filter, [FromQuery] string sort, 
                                                     [FromQuery] int page = 0, [FromQuery] int size = 10)
        {
            var serviceResponse = await lostDogService.GetLostDogs(filter, sort, page, size);
            var controllerResponse = mapper.Map<ServiceResponse<List<GetLostDogDto>, int>, ControllerResponse<List<GetLostDogDto>, int>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }


        [HttpGet]
        [Route("{dogId}")]
        public async Task<IActionResult> GetLostDogDetails(int dogId)
        {
            var serviceResponse = await lostDogService.GetLostDogDetails(dogId);
            var controllerResponse = mapper.Map<ServiceResponse<GetLostDogDto>, ControllerResponse<GetLostDogDto>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [HttpPut]
        [Route("{dogId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateLostDog([ModelBinder(BinderType = typeof(JsonModelBinder))] [FromForm] UploadLostDogDto dog,
                                                       IFormFile picture,
                                                       [FromRoute] int dogId)
        {
            var response = await lostDogService.GetLostDogDetails(dogId);
            if (!response.Successful)
                return StatusCode(response.StatusCode, response);
             
            if (User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value == response.Data.OwnerId.ToString())
            {
                dog.OwnerId = response.Data.OwnerId;
                var serviceResponse = await lostDogService.UpdateLostDog(dog, picture, dogId);
                var controllerResponse = mapper.Map<ServiceResponse<GetLostDogDto>, ControllerResponse<GetLostDogDto>>(serviceResponse);
                return StatusCode(serviceResponse.StatusCode, controllerResponse);
            }

            return Unauthorized(new ControllerResponse() 
                { 
                    Message = "Attempted to update a dog which is not owned by the user!", 
                    Successful = false
                });
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddLostDog([ModelBinder(BinderType = typeof(JsonModelBinder))] UploadLostDogDto dog,
                                                    IFormFile picture)
        {
            if (picture is null)
                return BadRequest( new ControllerResponse() 
                    { 
                        Message = "No image was provided!", 
                        Successful = false 
                    });
            dog.OwnerId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var serviceResponse = await lostDogService.AddLostDog(dog, picture);
            var controllerResponse = mapper.Map<ServiceResponse<GetLostDogDto>, ControllerResponse<GetLostDogDto>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }


        [HttpPut]
        [Route("{dogId}/found")]
        public async Task<IActionResult> MarkLostDogAsFound(int dogId)
        {
            var savedDog = await lostDogService.GetLostDogDetails(dogId);
            if (User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value == savedDog.Data.OwnerId.ToString())
            {
                var serviceResponse = await lostDogService.MarkLostDogAsFound(dogId);
                var controllerResponse = mapper.Map<ServiceResponse, ControllerResponse>(serviceResponse);
                return StatusCode(serviceResponse.StatusCode, controllerResponse);
            }
            else
                return Unauthorized(new ControllerResponse()
                {
                    Message = "Attempted to mark as found a dog which is not owned by the user!",
                    Successful = false
                });
        }


        [HttpDelete]
        [Route("{dogId}")]
        public async Task<IActionResult> DeleteLostDog(int dogId)
        {
            var savedDog = await lostDogService.GetLostDogDetails(dogId);
            if (User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value == savedDog.Data.OwnerId.ToString())
            {
                var serviceResponse = await lostDogService.DeleteLostDog(dogId);
                var controllerResponse = mapper.Map<ServiceResponse, ControllerResponse>(serviceResponse);
                return StatusCode(serviceResponse.StatusCode, controllerResponse);
            }
            else
                return Unauthorized(new ControllerResponse()
                {
                    Message = "Attempted to delete a dog which is not owned by the user!",
                    Successful = false
                });

        }

        //[HttpPost]
        //[Route("{}/comment")]
        //public async Task<IActionResult> AddLostDogComment(AddLostDogCommentDto commentDto)
        //{
        //    var serviceResponse = await _lostDogService.AddLostDogComment(commentDto);
        //    return StatusCode(serviceResponse.StatusCode, serviceResponse);
        //}
        //public async Task<IActionResult> EditLostDogComment(LostDogComment comment)
        //{
        //    var serviceResponse = await _lostDogService.EditLostDogComment(comment);
        //    return StatusCode(serviceResponse.StatusCode, serviceResponse);
        //}
        //public async Task<IActionResult> GetLostDogComments(int dogId)
        //{
        //    var serviceResponse = await _lostDogService.GetLostDogComments(dogId);
        //    return StatusCode(serviceResponse.StatusCode, serviceResponse);
        //}
    }
}

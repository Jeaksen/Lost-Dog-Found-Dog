using AutoMapper;
using Backend.DTOs.Dogs;
using Backend.Models.Authentication;
using Backend.Models.DogBase.LostDog;
using Backend.Models.Response;
using Backend.Services.LostDogService;
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
                                                     [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var serviceResponse = await lostDogService.GetLostDogs(filter, sort, page, size);
            var response = mapper.Map<ServiceResponse<List<LostDog>, int>, ControllerResponse<List<LostDog>, int>>(serviceResponse);
            foreach (var dog in serviceResponse.Data)
                dog.Picture.Data = null;

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }


        [HttpGet]
        [Route("{dogId}")]
        public async Task<IActionResult> GetLostDogDetails(int dogId)
        {
            var serviceResponse = await lostDogService.GetLostDogDetails(dogId);
            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

        [HttpPut]
        [Route("{dogId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateLostDog([ModelBinder(BinderType = typeof(JsonModelBinder))] [FromForm] UpdateLostDogDto dog,
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
                return StatusCode(serviceResponse.StatusCode, serviceResponse);
            }

            return Unauthorized(new ServiceResponse<bool>() 
                    { 
                        Message = "Attempted to update a dog which is not owned by the user!", 
                        Successful = false, 
                        StatusCode = StatusCodes.Status401Unauthorized 
                    });
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddLostDog([ModelBinder(BinderType = typeof(JsonModelBinder))] AddLostDogDto dog,
                                                    IFormFile picture)
        {
            if (picture is null)
                return BadRequest( new ServiceResponse<bool>() 
                    { 
                        Message = "No image was provided!", 
                        Successful = false, 
                        StatusCode = StatusCodes.Status400BadRequest 
                    });
            dog.OwnerId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var serviceResponse = await lostDogService.AddLostDog(dog, picture);
            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }


        [HttpPut]
        [Route("{dogId}/found")]
        public async Task<IActionResult> MarkLostDogAsFound(int dogId)
        {
            var serviceResponse = await lostDogService.MarkLostDogAsFound(dogId);
            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }


        [HttpDelete]
        [Route("{dogId}")]
        public async Task<IActionResult> DeleteLostDog(int dogId)
        {
            var serviceResponse = await lostDogService.DeleteLostDog(dogId);
            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }


        //[HttpGet]
        //[Route]
        //public async Task<IActionResult> GetUserLostDogs(int ownerId)
        //{
        //    var serviceResponse = await _lostDogService.GetUserLostDogs(ownerId);
        //    return StatusCode(serviceResponse.StatusCode, serviceResponse);
        //}
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

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
            var controllerResponse = mapper.Map<ControllerResponse<List<GetLostDogDto>, int>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }


        [HttpGet]
        [Route("{dogId}")]
        public async Task<IActionResult> GetLostDogDetails(int dogId)
        {
            var serviceResponse = await lostDogService.GetLostDogDetails(dogId);
            var controllerResponse = mapper.Map<ControllerResponse<GetLostDogWithCommentsDto>>(serviceResponse);

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
                var controllerResponse = mapper.Map<ControllerResponse<GetLostDogDto>>(serviceResponse);
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
            var controllerResponse = mapper.Map<ControllerResponse<GetLostDogDto>>(serviceResponse);

            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }


        [HttpPut]
        [Route("{dogId}/found")]
        public async Task<IActionResult> MarkLostDogAsFound(int dogId)
        {
            var savedDogResponse = await lostDogService.GetLostDogDetails(dogId);
            if (savedDogResponse.Data == null)
                return StatusCode(savedDogResponse.StatusCode, mapper.Map<ControllerResponse<GetLostDogDto>>(savedDogResponse));

            if (User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value == savedDogResponse.Data.OwnerId.ToString())
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
            var savedDogResponse = await lostDogService.GetLostDogDetails(dogId);
            if (savedDogResponse.Data == null)
                return StatusCode(savedDogResponse.StatusCode, mapper.Map<ControllerResponse<GetLostDogDto>>(savedDogResponse));

            if (User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value == savedDogResponse.Data.OwnerId.ToString())
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

        [HttpPost]
        [Route("{dogId}/comments")]
        public async Task<IActionResult> AddLostDogComment([ModelBinder(BinderType = typeof(JsonModelBinder))][FromForm] UploadCommentDto comment,
                                                           IFormFile picture,
                                                           [FromRoute] int dogId)
        {
            comment.AuthorId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            comment.DogId = dogId;

            var serviceResponse = await lostDogService.AddLostDogComment(comment, picture);
            var controllerResponse = mapper.Map<ControllerResponse<GetCommentDto>>(serviceResponse);
            return StatusCode(serviceResponse.StatusCode, controllerResponse);
        }

        [HttpDelete]
        [Route("{dogId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteLostDogComment([FromRoute] int dogId, [FromRoute] int commentId)
        {
            var savedComment = await lostDogService.GetLostDogComment(dogId, commentId);
            if (!savedComment.Successful)
                return StatusCode(savedComment.StatusCode, mapper.Map<ControllerResponse>(savedComment));

            if (User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value == savedComment.Data.Author.Id.ToString())
            {
                var serviceResponse = await lostDogService.DeleteLostDogComment(dogId, commentId);
                var controllerResponse = mapper.Map<ControllerResponse>(serviceResponse);
                return StatusCode(serviceResponse.StatusCode, controllerResponse);
            }
            else
                return Unauthorized(new ControllerResponse()
                {
                    Message = "Attempted to delete a comment which is not owned by the user!",
                    Successful = false
                });
        }


    }
}

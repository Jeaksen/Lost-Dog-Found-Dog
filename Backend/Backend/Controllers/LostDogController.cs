using Backend.DTOs.Dogs;
using Backend.Models.Authentication;
using Backend.Models.DogBase.LostDog;
using Backend.Services;
using Backend.Services.LostDogService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    
    [Authorize(Roles = AccountRoles.User)]
    [Route("/lostdogs/")]
    [ApiController]
    public class LostDogController : ControllerBase
    {
        private readonly ILostDogService lostDogService;

        public LostDogController(ILostDogService lostDogService)
        {
            this.lostDogService = lostDogService;
        }


        [HttpGet]
        public async Task<IActionResult> GetLostDogs(int? ownerId)
        {
            ServiceResponse<List<LostDog>> serviceResponse;
            if (ownerId.HasValue)
                serviceResponse = await lostDogService.GetUserLostDogs(ownerId.Value);
            else
                serviceResponse = await lostDogService.GetLostDogs();

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
        public async Task<IActionResult> UpdateLostDog(IFormCollection form, IFormFile picture, [FromRoute] int dogId)
        {
            var updateLostDogDto = new UpdateLostDogDto();
            var formValueProvider = new FormValueProvider(BindingSource.Form, form, CultureInfo.CurrentCulture);
            var bindingSuccessful = await TryUpdateModelAsync(updateLostDogDto, "", formValueProvider);

            if (!bindingSuccessful)
            {
                var responseBuilder = new StringBuilder("Failed to bind UpdateLostDogDto: ");
                foreach (var modelState in ModelState.Values)
                    foreach (var error in modelState.Errors)
                        responseBuilder.Append(error.ErrorMessage);

                return StatusCode(StatusCodes.Status400BadRequest,
                    new ServiceResponse<bool>() { Message = responseBuilder.ToString(), Successful = false, StatusCode = StatusCodes.Status400BadRequest });
            }

            var serviceResponse = await lostDogService.UpdateLostDog(updateLostDogDto, picture, dogId);
            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddLostDog(IFormCollection form, IFormFile picture)
        {
            var addLostDogDto = new AddLostDogDto();
            var formValueProvider = new FormValueProvider(BindingSource.Form, form, CultureInfo.CurrentCulture);
            var bindingSuccessful = await TryUpdateModelAsync(addLostDogDto, "", formValueProvider);

            if (picture == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ServiceResponse<bool>() { Message = "No image was provided!", Successful = false, StatusCode = StatusCodes.Status400BadRequest });
            }
            
            if (!bindingSuccessful)
            {
                var responseBuilder = new StringBuilder("Failed to bind AddLostDogDto: ");
                foreach (var modelState in ModelState.Values)
                    foreach (var error in modelState.Errors)
                        responseBuilder.Append(error.ErrorMessage);

                return StatusCode(StatusCodes.Status400BadRequest, 
                    new ServiceResponse<bool>() { Message = responseBuilder.ToString(), Successful = false, StatusCode = StatusCodes.Status400BadRequest });
            }

            var serviceResponse = await lostDogService.AddLostDog(addLostDogDto, picture);
            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

        [Authorize(Policy ="Edit")]
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

﻿using Backend.DTOs.Dogs;
using Backend.Models.Authentication;
using Backend.Models.DogBase.LostDog;
using Backend.Services;
using Backend.Services.LostDogService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Authorize(Roles = AccountRoles.User)]
    [Route("/lostdogs/")]
    [ApiController]
    public class LostDogController : ControllerBase
    {
        private readonly ILostDogService _lostDogService;

        public LostDogController(ILostDogService lostDogService)
        {
            _lostDogService = lostDogService;
        }

        [HttpPost]
        public async Task<IActionResult> AddLostDog(AddLostDogDto lostDogDto)
        {
            var serviceResponse = await _lostDogService.AddLostDog(lostDogDto);
            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

        //[HttpPost]
        //[Route("{}/comment")]
        //public async Task<IActionResult> AddLostDogComment(AddLostDogCommentDto commentDto)
        //{
        //    var serviceResponse = await _lostDogService.AddLostDogComment(commentDto);
        //    return StatusCode(serviceResponse.StatusCode, serviceResponse);
        //}

        [HttpDelete]
        [Route("{dogId}")]
        public async Task<IActionResult> DeleteLostDog(int dogId)
        {
            var serviceResponse = await _lostDogService.DeleteLostDog(dogId);
            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

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

        [HttpGet]
        [Route("{dogId}")]
        public async Task<IActionResult> GetLostDogDetails(int dogId)
        {
            var serviceResponse = await _lostDogService.GetLostDogDetails(dogId);
            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetLostDogs()
        {
            var serviceResponse = await _lostDogService.GetLostDogs();
            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

        //[HttpGet]
        //[Route]
        //public async Task<IActionResult> GetUserLostDogs(int ownerId)
        //{
        //    var serviceResponse = await _lostDogService.GetUserLostDogs(ownerId);
        //    return StatusCode(serviceResponse.StatusCode, serviceResponse);
        //}
    }
}
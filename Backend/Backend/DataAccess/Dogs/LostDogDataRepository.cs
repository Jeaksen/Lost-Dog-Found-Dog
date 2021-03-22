using AutoMapper;
using Backend.DTOs.Dogs;
using Backend.Models.DogBase.LostDog;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Backend.DataAccess.Dogs
{
    public class LostDogDataRepository : ILostDogRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LostDogDataRepository> _logger;

        public LostDogDataRepository(ApplicationDbContext dbContext, IMapper mapper, ILogger<LostDogDataRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }


        public LostDog AddLostDog(AddLostDogDto lostDogDto, IFormFile image)
        {
            var lostDog = _mapper.Map<LostDog>(lostDogDto);
            throw new NotImplementedException();
        }

        public bool DeleteLostDog(int dogId)
        {
            throw new NotImplementedException();
        }

        public LostDog GetLostDogDetails(int dogId)
        {
            throw new NotImplementedException();
        }

        public List<LostDog> GetLostDogs()
        {
            throw new NotImplementedException();
        }

        public LostDog GetUserLostDogs(int ownerId)
        {
            throw new NotImplementedException();
        }

        public List<LostDogComment> GetLostDogComments(int dogId)
        {
            throw new NotImplementedException();
        }

        public LostDogComment EditLostDogComment(LostDogComment comment, int dogId)
        {
            throw new NotImplementedException();
        }

        public LostDogComment AddLostDogComment(AddLostDogCommentDto comment, IFormFile image, int dogId)
        {
            throw new NotImplementedException();
        }
    }
}

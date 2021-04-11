using Backend.DTOs.Dogs;
using Backend.Models.DogBase.LostDog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DataAccess.Dogs
{
    public class LostDogDataRepository : ILostDogRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<LostDogDataRepository> logger;

        public LostDogDataRepository(ApplicationDbContext dbContext, ILogger<LostDogDataRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<RepositoryResponse<LostDog>> AddLostDog(LostDog lostDog)
        {
            var response = new RepositoryResponse<LostDog>();
            try
            {
                var returningDog = await dbContext.LostDogs.AddAsync(lostDog);
                await dbContext.SaveChangesAsync();
                response.Data = returningDog.Entity;
                response.Message = $"Lost Dog was added with id {returningDog.Entity.Id}";
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to delete dog: {e.Message}";
            }

            return response;
        }

        public async Task<RepositoryResponse<List<LostDog>>> GetLostDogs()
        {
            var response = new RepositoryResponse<List<LostDog>>();
            try
            {
                var lostDogs = await dbContext.LostDogs
                            .Include(dog => dog.Behaviors)
                            .Include(dog => dog.Picture)
                            .Include(dog => dog.Comments)
                            .Include(dog => dog.Location)
                            .ToListAsync();
                response.Data = lostDogs;
                response.Message = $"Found {lostDogs.Count} Lost Dogs";
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to get lost dogs: {e.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<List<LostDog>>> GetUserLostDogs(int ownerId)
        {
            var response = new RepositoryResponse<List<LostDog>>();
            try
            {
                var lostDogs = await dbContext.LostDogs.Where(ld => ld.OwnerId == ownerId)
                            .Include(dog => dog.Behaviors)
                            .Include(dog => dog.Picture)
                            .Include(dog => dog.Comments)
                            .Include(dog => dog.Location)
                            .ToListAsync();
                response.Data = lostDogs;
                response.Message = $"Found {lostDogs.Count} Lost Dogs";
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to get lost dogs for user {ownerId}: {e.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<LostDog>> GetLostDogDetails(int dogId)
        {
            var response = new RepositoryResponse<LostDog>();
            try
            {
                var dog = await dbContext.LostDogs.FindAsync(dogId);
                if (dog == null)
                {
                    response.Successful = false;
                    response.Message = $"Dog with id {dogId} was not found";
                }
                else
                {
                    response.Data = dog;
                    response.Message = $"Lost Dog with id {dogId} was found";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = "Failed to find dog: " + e.Message;
            }
            return response;
        }

        public async Task<RepositoryResponse<LostDog>> UpdateLostDog(LostDog lostDog)
        {
            var response = new RepositoryResponse<LostDog>();
            try
            {
                var dog = await dbContext.LostDogs.FindAsync(lostDog.Id);
                if (dog == null)
                {
                    response.Successful = false;
                    response.Message = $"Dog with id {lostDog.Id} was not found";
                }
                else
                {
                    // The object is tracked after find, so it has to be detached
                    dbContext.Entry(dog).State = EntityState.Detached;
                    var updatedDog = dbContext.LostDogs.Update(lostDog);
                    await dbContext.SaveChangesAsync();
                    response.Data = updatedDog.Entity;
                    response.Successful = true;
                    response.Message = "Dog updated successfully";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = "Failed to update dog: " + e.Message;
            }
            return response;
        }

        public async Task<RepositoryResponse<bool>> MarkDogAsFound(int dogId)
        {
            var response = new RepositoryResponse<bool>();
            try
            {
                var lostDog = await dbContext.LostDogs.FindAsync(dogId);
                if (lostDog == null)
                {
                    response.Successful = false;
                    response.Message = $"Dog with {dogId} was not found";
                }
                else if (lostDog.IsFound)
                {
                    response.Successful = false;
                    response.Message = $"Dog with {dogId} is already marked as signed";
                }
                else
                {
                    lostDog.IsFound = true;
                    dbContext.SaveChanges();
                    response.Data = true;
                    response.Message = $"Lost Dog with id {dogId} was marked as found";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = "Failed to mark dog as found: " + e.Message;
            }
            return response;
        }

        public async Task<RepositoryResponse<bool>> DeleteLostDog(int dogId)
        {
            var response = new RepositoryResponse<bool>();
            try
            {
                var lostDog = await dbContext.LostDogs.FindAsync(dogId);
                if (lostDog == null)
                {
                    response.Successful = false;
                    response.Message = $"Failed to find dog {dogId}";
                } 
                else
                {
                    dbContext.LostDogs.Remove(lostDog);
                    dbContext.SaveChanges();
                    response.Data = true;
                    response.Message = $"Lost Dog with id {dogId} was deleted";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to delete dog: {e.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<LostDogComment>> AddLostDogComment(LostDogComment comment)
        {
            var response = new RepositoryResponse<LostDogComment>();
            try
            {
                var lostDog = await dbContext.LostDogs.FindAsync(comment.DogId);
                if (lostDog == null)
                {
                    response.Successful = false;
                    response.Message = $"Failed to find dog with id {comment.DogId}";
                } 
                else
                {
                    lostDog.Comments.Add(comment);
                    dbContext.SaveChanges();
                    // Does the ID change?
                    response.Data = lostDog.Comments.Last();
                    response.Message = $"Comment for Lost Dog with id {comment.DogId} was added";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to delete dog: {e.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<List<LostDogComment>>> GetLostDogComments(int dogId)
        {
            var response = new RepositoryResponse<List<LostDogComment>> ();
            try
            {
                var comments =  await dbContext.LostDogComments.Where(c => c.DogId == dogId).ToListAsync();
                response.Data = comments;
                response.Message = $"Found {comments.Count} Lost Dogs Comments";
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to delete dog: {e.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<LostDogComment>> EditLostDogComment(LostDogComment comment)
        {
            var response = new RepositoryResponse<LostDogComment>();
            try
            {
                var lostDog = await dbContext.LostDogs.FindAsync(comment.DogId);
                if (lostDog == null)
                {
                    response.Successful = false;
                    response.Message = $"Failed to find dog {comment.DogId}";
                }
                else
                {
                    int index = lostDog.Comments.FindIndex(c => c.Id == comment.Id);
                    if (index == -1)
                    {
                        response.Successful = false;
                        response.Message = $"Failed to comment for dog {comment.DogId} with id {comment.Id}";
                    } 
                    else
                    {
                        lostDog.Comments[index] = comment;
                        dbContext.SaveChanges();
                        response.Data = comment;
                        response.Message = $"Comment for Lost Dog with id {comment.DogId} was edited";
                    }
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to delete dog: {e.Message}";
            }
            return response;
        }
    }
}

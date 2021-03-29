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

        public async Task<LostDog> AddLostDog(LostDog lostDog)
        {
            try
            {
                var returningDog = await dbContext.LostDogs.AddAsync(lostDog);
                await dbContext.SaveChangesAsync();
                return returningDog.Entity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteLostDog(int dogId)
        {
            try
            {
                var lostDog = await dbContext.LostDogs.FindAsync(dogId);
                if (lostDog == null) throw new Exception();
                dbContext.LostDogs.Remove(lostDog);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> MarkDogAsFound(int dogId)
        {
            try
            {
                var lostDog = await dbContext.LostDogs.FindAsync(dogId);
                if (lostDog == null || lostDog.IsFound)
                    return false;
                lostDog.IsFound = true;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<LostDog> GetLostDogDetails(int dogId)
        {
            return await dbContext.LostDogs.FindAsync(dogId);
        }

        public async Task<List<LostDog>> GetLostDogs()
        {
            try
            {
                return await dbContext.LostDogs
                            .Include(dog => dog.Behaviors)
                            .Include(dog => dog.Picture)
                            .Include(dog => dog.Comments)
                            .Include(dog => dog.Location)
                            .ToListAsync();
                    
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<LostDog>> GetUserLostDogs(int ownerId)
        {
            try
            {
                return await dbContext.LostDogs.Where(ld => ld.OwnerId == ownerId)
                            .Include(dog => dog.Behaviors)
                            .Include(dog => dog.Picture)
                            .Include(dog => dog.Comments)
                            .Include(dog => dog.Location)
                            .ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<LostDogComment> AddLostDogComment(LostDogComment comment)
        {
            try
            {
                var lostDog = await dbContext.LostDogs.FindAsync(comment.DogId);
                if (lostDog == null) throw new Exception();
                lostDog.Comments.Add(comment);
                dbContext.SaveChanges();
                return comment;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<LostDogComment>> GetLostDogComments(int dogId)
        {
            try
            {
                return (await dbContext.LostDogComments.Where(c => c.DogId == dogId).ToListAsync());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<LostDogComment> EditLostDogComment(LostDogComment comment)
        {
            try
            {
                // Comments may be null?
                var lostDog = await dbContext.LostDogs.FindAsync(comment.DogId);
                if (lostDog == null) throw new Exception();
                var oldComment = lostDog.Comments.Find(c => c.Id == comment.Id);
                if (oldComment == null) throw new Exception();
                oldComment = comment;
                dbContext.SaveChanges();
                return comment;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

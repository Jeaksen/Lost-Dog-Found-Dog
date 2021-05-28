using Backend.DataAccess.LostDogs;
using Backend.Models.Dogs.ShelterDogs;
using Backend.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DataAccess.ShelterDogs
{
    public class ShelterDogDataRepository : IShelterDogRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<ShelterDogDataRepository> logger;

        public ShelterDogDataRepository(ApplicationDbContext dbContext, ILogger<ShelterDogDataRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<RepositoryResponse<ShelterDog>> AddShelterDog(ShelterDog shelterDog)
        {
            var response = new RepositoryResponse<ShelterDog>();
            try
            {
                if (shelterDog.Picture == null)
                    throw new ArgumentException("ShelterDog picture can not be null");
                var returningDog = await dbContext.ShelterDogs.AddAsync(shelterDog);
                await dbContext.SaveChangesAsync();
                response.Data = returningDog.Entity;
                response.Message = $"Shelter Dog was added with id {returningDog.Entity.Id}";
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to add dog: {e.Message} {e.InnerException?.Message}";
            }

            return response;
        }

        public async Task<RepositoryResponse> DeleteShelterDog(int dogId)
        {
            var response = new RepositoryResponse();
            try
            {
                var shelterDog = await dbContext.ShelterDogs.FindAsync(dogId);
                if (shelterDog == null)
                {
                    response.Successful = false;
                    response.Message = $"Failed to find dog {dogId}";
                }
                else
                {
                    dbContext.ShelterDogs.Remove(shelterDog);
                    dbContext.SaveChanges();
                    response.Message = $"Shelter Dog with id {dogId} was deleted";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to delete dog: {e.Message}  {e.InnerException?.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<ShelterDog>> GetShelterDogDetails(int dogId)
        {
            var response = new RepositoryResponse<ShelterDog>();
            try
            {
                var dog = await dbContext.ShelterDogs
                                            .Where(ld => ld.Id == dogId)
                                            .Include(dog => dog.Behaviors)
                                            .Include(dog => dog.Picture)
                                            .SingleOrDefaultAsync();
                if (dog == default)
                {
                    response.Successful = false;
                    response.Message = $"Dog with id {dogId} was not found";
                }
                else
                {
                    response.Data = dog;
                    response.Message = $"Shelter Dog with id {dogId} was found";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to find dog: {e.Message}  {e.InnerException?.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<List<ShelterDog>, int>> GetShelterDogs(int shelterId, int page, int size)
        {
            var response = new RepositoryResponse<List<ShelterDog>, int>();
            try
            {
                var query = dbContext.ShelterDogs
                            .Where(dog => dog.ShelterId == shelterId)
                            .Include(dog => dog.Behaviors)
                            .Include(dog => dog.Picture);

                response.Metadata = (int)Math.Ceiling(await query.CountAsync() / (double)size);
                response.Data = await query.Skip(page * size).Take(size).ToListAsync();
                response.Message = $"Found {response.Data.Count} Shelter Dogs";
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to get shelter dogs: {e.Message} {e.InnerException?.Message}";
            }
            return response;
        }
    }
}

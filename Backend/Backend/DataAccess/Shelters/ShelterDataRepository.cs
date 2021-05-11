using Backend.Models.Response;
using Backend.Models.Shelters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DataAccess.Shelters
{
    public class ShelterDataRepository : IShelterRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<ShelterDataRepository> logger;

        public ShelterDataRepository(ApplicationDbContext dbContext, ILogger<ShelterDataRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<RepositoryResponse<Shelter>> AddShelter(Shelter shelter)
        {
            var response = new RepositoryResponse<Shelter>();
            try
            {
                var addedShelter = await dbContext.Shelters.AddAsync(shelter);
                await dbContext.SaveChangesAsync();
                response.Message = $"Shelter was added with id {addedShelter.Entity.Id}";
                response.Data = addedShelter.Entity;
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to delete dog: {e.Message} {e.InnerException?.Message}";
            }

            return response;
        }

        public async Task<RepositoryResponse<Shelter>> GetShelter(int id)
        {
            var response = new RepositoryResponse<Shelter>();
            try
            {
                var shelter = await dbContext.Shelters
                                            .Where(s => s.Id == id)
                                            .Include(s => s.Address)
                                            .SingleOrDefaultAsync();
                if (shelter == default)
                {
                    response.Successful = false;
                    response.Message = $"Shelter with id {id} was not found";
                }
                else
                {
                    response.Data = shelter;
                    response.Message = $"Lost Dog with id {id} was found";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to find shelter: {e.Message}  {e.InnerException?.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse> DeleteShelterWithoutDogs(int id)
        {
            var response = new RepositoryResponse();
            try
            {
                var getResponse = await GetShelter(id);
                if (response.Successful)
                {
                    dbContext.Shelters.Remove(getResponse.Data);
                    await dbContext.SaveChangesAsync();
                    response.Message = $"Shelter with id {id} was deleted";
                }
                else
                {
                    response.Successful = false;
                    response.Message = getResponse.Message;
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to delete shelter: {e.Message}  {e.InnerException?.Message}";
            }
            return response;
        }
    }
}

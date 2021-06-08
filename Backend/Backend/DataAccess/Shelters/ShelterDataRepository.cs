using Backend.Models.Response;
using Backend.Models.Shelters;
using DynamicExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
                if (shelter.Address == null)
                    throw new ArgumentException("Shelter address can not be null");
                var addedShelter = await dbContext.Shelters.AddAsync(shelter);
                await dbContext.SaveChangesAsync();
                response.Message = $"Shelter was added with id {addedShelter.Entity.Id}";
                response.Data = addedShelter.Entity;
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed add shelter: {e.Message} {e.InnerException?.Message}";
            }

            return response;
        }

        public async Task<RepositoryResponse<Shelter>> ApproveShelter(int id)
        {
            var response = await GetShelterApprovalInvariant(id);

            if (response.Successful)
            {
                try
                {
                    if (response.Data.IsApproved)
                        throw new Exception("The shelter is already approved!");
                    response.Data.IsApproved = true;
                    await dbContext.SaveChangesAsync();
                    response.Message = $"shelter with id {id} was approved";
                }
                catch (Exception e)
                {
                    response.Successful = false;
                    response.Message = $"Failed to accept shelter with id {id}: {e.Message}  {e.InnerException?.Message}";
                }
            }

            return response;
        }

        public async Task<RepositoryResponse<List<Shelter>, int>> GetShelters(string name, string sort, int page, int size)
        {
            var response = new RepositoryResponse<List<Shelter>, int>();
            try
            {
                IOrderedQueryable<Shelter> ordered;
                var query = dbContext.Shelters.Where(s => s.IsApproved).Include(s => s.Address).AsQueryable();

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.Name.StartsWith(name));

                if (!string.IsNullOrEmpty(sort) && sort.Equals("name,desc", StringComparison.InvariantCultureIgnoreCase))
                    ordered = query.OrderByDescending(s => s.Name);
                else
                    ordered = query.OrderBy(s => s.Name);

                response.Metadata = (int)Math.Ceiling(await ordered.CountAsync() / (double)size);
                response.Data = await ordered.Skip(page * size).Take(size).ToListAsync();
                response.Message = $"Found {response.Data.Count} shelters";
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to get lost dogs: {e.Message} {e.InnerException?.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<Shelter>> GetShelter(int id)
        {
            var response = new RepositoryResponse<Shelter>();
            try
            {
                var shelter = await dbContext.Shelters
                                            .Where(s => s.Id == id && s.IsApproved)
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
                    response.Message = $"Shelter with id {id} was found";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to find shelter: {e.Message}  {e.InnerException?.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<Shelter>> GetShelterApprovalInvariant(int id)
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
                    response.Message = $"Shelter with id {id} was found";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to find shelter: {e.Message}  {e.InnerException?.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse> DeleteShelter(int id)
        {
            var response = new RepositoryResponse();
            try
            {
                var getResponse = await GetShelter(id);
                if (!getResponse.Successful)
                    throw new Exception(getResponse.Message);

                dbContext.Remove(getResponse.Data);
                await dbContext.SaveChangesAsync();
                response.Message = $"Shelter with id {id} was deleted";
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

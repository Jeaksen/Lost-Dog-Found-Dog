using Backend.Models.DogBase.LostDog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Util;
using DynamicExpressions;
using System.Reflection;
using System.Linq.Expressions;
using Backend.Models.Response;

namespace Backend.DataAccess.Dogs
{
    public class LostDogDataRepository : ILostDogRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<LostDogDataRepository> logger;

        private static readonly Dictionary<string, FilterOperator> filterOperatorsForProperties = new Dictionary<string, FilterOperator>()
        {
            { "Breed", FilterOperator.Equals },
            { "AgeFrom", FilterOperator.GreaterThanOrEqual },
            { "AgeTo", FilterOperator.LessThanOrEqual },
            { "Size", FilterOperator.Equals },
            { "Color", FilterOperator.Equals },
            { "Name", FilterOperator.Equals },
            { "OwnerId", FilterOperator.Equals },
            { "City", FilterOperator.Equals },
            { "District", FilterOperator.Equals },
            { "DateLostBefore", FilterOperator.LessThanOrEqual },
            { "DateLostAfter", FilterOperator.GreaterThanOrEqual }
        };

        private static readonly Dictionary<string, string> lostDogPropertyForFilterProperty = new Dictionary<string, string>()
        {
            { "Breed", "Breed" },
            { "AgeFrom", "Age" },
            { "AgeTo", "Age" },
            { "Size", "Size" },
            { "Color", "Color" },
            { "Name", "Name" },
            { "City", "Location.City" },
            { "District", "Location.District" },
            { "DateLostBefore", "DateLost" },
            { "DateLostAfter", "DateLost" },
            { "OwnerId", "OwnerId" }
        };

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
                response.Message = $"Failed to delete dog: {e.Message} {e.InnerException?.Message}";
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
                response.Message = $"Failed to get lost dogs: {e.Message} {e.InnerException?.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<List<LostDog>, int>> GetLostDogs(LostDogFilter filter, string sort, int page, int size)
        {
            var response = new RepositoryResponse<List<LostDog>, int>();
            try
            {
                var predicateBuilder = new DynamicFilterBuilder<LostDog>();
                var filterProperties = GetNotNullProperties(filter);

                if (filterProperties.Remove(filter.GetType().GetProperty("Location")))
                    foreach (var result in GetNotNullProperties(filter.Location))
                        predicateBuilder.And(lostDogPropertyForFilterProperty[result.Name], filterOperatorsForProperties[result.Name], result.GetValue(filter.Location));
                
                foreach (var result in filterProperties)
                    predicateBuilder.And(lostDogPropertyForFilterProperty[result.Name], filterOperatorsForProperties[result.Name], result.GetValue(filter));

                Expression<Func<LostDog, bool>> predicate = (LostDog l) => true;
                try { predicate = predicateBuilder.Build(); }
                catch (Exception) {}

                var query = dbContext.LostDogs
                            .Where(predicate)
                            .Include(dog => dog.Behaviors)
                            .Include(dog => dog.Picture)
                            .Include(dog => dog.Comments)
                            .Include(dog => dog.Location);

                IOrderedQueryable<LostDog> ordered = null;

                if (!string.IsNullOrEmpty(sort))
                {
                    var split = sort.Split(',');
                    var propertyGetter = DynamicExpressions.DynamicExpressions.GetPropertyGetter<LostDog>(split[0]);
                    if (split.Length > 1)
                    {
                        if (string.Equals(split[1], "ASC", StringComparison.InvariantCultureIgnoreCase))
                            ordered = query.OrderBy(propertyGetter);
                        else if (string.Equals(split[1], "DESC", StringComparison.InvariantCultureIgnoreCase))
                            ordered = query.OrderByDescending(propertyGetter);
                        else
                            throw new ArgumentException($"Invalid ordering type: {split[1]} for parameter {split[0]}");
                    }
                    else    
                        ordered = query.OrderBy(propertyGetter);
                }
                else
                    ordered = query.OrderByDescending(d => d.DateLost);

                response.Data = await ordered.Skip((page - 1) * size).Take(size).ToListAsync();
                response.Metadata = await ordered.CountAsync();
                response.Message = $"Found {response.Data.Count} Lost Dogs";
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to get lost dogs: {e.Message} {e.InnerException?.Message}";
            }
            return response;
        }

        private List<PropertyInfo> GetNotNullProperties<T>(T obj) => obj.GetType().GetProperties().Where(p => p.GetValue(obj) != null).ToList();

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
                response.Message = $"Failed to get lost dogs for user {ownerId}: {e.Message} {e.InnerException?.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<LostDog>> GetLostDogDetails(int dogId)
        {
            var response = new RepositoryResponse<LostDog>();
            try
            {
                var dog = await dbContext.LostDogs
                                            .Where(ld => ld.Id == dogId)
                                            .Include(dog => dog.Behaviors)
                                            .Include(dog => dog.Picture)
                                            .Include(dog => dog.Comments)
                                            .Include(dog => dog.Location)
                                            .SingleOrDefaultAsync();
                if (dog == default)
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
                response.Message = $"Failed to find dog: {e.Message}  {e.InnerException?.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse<LostDog>> UpdateLostDog(LostDog updatedDog)
        {
            var response = new RepositoryResponse<LostDog>();
            try
            {
                var detailsResponse = await GetLostDogDetails(updatedDog.Id);
                if (!detailsResponse.Successful)
                    return detailsResponse;
                else
                {
                    var savedDog = detailsResponse.Data;
                    if (updatedDog.Picture is not null)
                    {
                        dbContext.Pictures.Remove(savedDog.Picture);
                        savedDog.Picture = updatedDog.Picture;
                    }
                    if (!updatedDog.Location.Equals(savedDog.Location))
                    {
                        dbContext.Locations.Remove(savedDog.Location);
                        savedDog.Location = updatedDog.Location;
                    }

                    var commonBehaviors = updatedDog.Behaviors.Intersect(savedDog.Behaviors).ToList();
                    var addBehaviors = updatedDog.Behaviors.Except(commonBehaviors).ToList();
                    var removeBehaviorsIndices = savedDog.Behaviors.Except(commonBehaviors).Select(b => b.Id).ToList();

                    foreach (int v in removeBehaviorsIndices)
                        savedDog.Behaviors.RemoveAll(b => b.Id == v);
                    savedDog.Behaviors.AddRange(addBehaviors);

                    //all properties are copied from lostDog to savedDog, so these ones must be assigned to lostDog
                    updatedDog.Behaviors = savedDog.Behaviors;
                    updatedDog.Comments = savedDog.Comments;
                    updatedDog.Picture = savedDog.Picture;
                    updatedDog.Location = savedDog.Location;
                    updatedDog.CopyProperties(savedDog);

                    await dbContext.SaveChangesAsync();
                    response.Data = savedDog;
                    response.Message = "Dog updated successfully";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to update dog: {e.Message}  {e.InnerException?.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse> MarkDogAsFound(int dogId)
        {
            var response = new RepositoryResponse();
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
                    response.Message = $"Lost Dog with id {dogId} was marked as found";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to mark dog as found: {e.Message}  {e.InnerException?.Message}";
            }
            return response;
        }

        public async Task<RepositoryResponse> DeleteLostDog(int dogId)
        {
            var response = new RepositoryResponse();
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
                    response.Message = $"Lost Dog with id {dogId} was deleted";
                }
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.Message = $"Failed to delete dog: {e.Message}  {e.InnerException?.Message}";
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
                response.Message = $"Failed to delete dog: {e.Message}  {e.InnerException?.Message}";
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
                response.Message = $"Failed to delete dog: {e.Message}  {e.InnerException?.Message}";
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
                response.Message = $"Failed to delete dog: {e.Message}  {e.InnerException?.Message}";
            }
            return response;
        }

    }
}

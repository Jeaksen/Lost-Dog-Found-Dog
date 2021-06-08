using Backend.Models.Response;
using Backend.Models.Shelters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.DataAccess.Shelters
{
    public interface IShelterRepository
    {
        public Task<RepositoryResponse<Shelter>> AddShelter(Shelter shelter);

        public Task<RepositoryResponse<Shelter>> ApproveShelter(int id);

        public Task<RepositoryResponse<Shelter>> GetShelter(int id);

        public Task<RepositoryResponse<Shelter>> GetShelterApprovalInvariant(int id);

        public Task<RepositoryResponse> DeleteShelter(int id);

        public Task<RepositoryResponse<List<Shelter>, int>> GetShelters(string name, string sort, int page, int size);
    }
}

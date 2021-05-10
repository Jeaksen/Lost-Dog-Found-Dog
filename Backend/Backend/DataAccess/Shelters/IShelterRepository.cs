using Backend.Models.Response;
using Backend.Models.Shelters;
using System.Threading.Tasks;

namespace Backend.DataAccess.Shelters
{
    public interface IShelterRepository
    {
        public Task<RepositoryResponse<Shelter>> AddShelter(Shelter shelter);

        public Task<RepositoryResponse<Shelter>> GetShelter(int id);
    }
}

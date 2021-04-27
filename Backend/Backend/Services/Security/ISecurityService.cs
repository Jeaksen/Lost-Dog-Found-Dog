using Backend.Models.Response;
using Microsoft.AspNetCore.Http;

namespace Backend.Services.Security
{
    public interface ISecurityService
    {
        public ServiceResponse IsPictureValid(IFormFile picture);
    }
}

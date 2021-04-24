using Microsoft.AspNetCore.Http;

namespace Backend.Services.Security
{
    public interface ISecurityService
    {
        public (bool Successful, string Message) IsPictureValid(IFormFile picture);
    }
}

using Backend.Models.Response;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Services.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly int MinimalPictureSize = 1024;
        private readonly int MaximalPictureSize = 10 * 1024 * 1024; // 10MB
        private readonly List<string> AllowedPictureMimeTypes = new() { "image/png", "image/jpeg" };
        private readonly Dictionary<string, List<string>> ExtensionsForMimeType = new()
        {
            { "image/png", new List<string>() { "png"} },
            { "image/jpeg", new List<string>() { "jpg", "jpeg" } }
        };

        public ServiceResponse IsPictureValid(IFormFile picture)
        {
            var response = new ServiceResponse();
            if (picture.Length < MinimalPictureSize)
            {
                response.Message = $"Picture has size smaller than {MinimalPictureSize / 1024} KB!";
                response.Successful = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else if (picture.Length > MaximalPictureSize)
            {
                response.Message = $"Picture has size bigger than {MaximalPictureSize / 1024 / 1024} MB!";
                response.Successful = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                var typeIndex = AllowedPictureMimeTypes.FindIndex(s => s == picture.ContentType);
                if (!AllowedPictureMimeTypes.Contains(picture.ContentType))
                {
                    response.Message = $"Type {picture.ContentType} is not an allowed type for pictures!";
                    response.Successful = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                }
                else
                {
                    var ext = picture.FileName.Split('.').Last();
                    if (!ExtensionsForMimeType[picture.ContentType].Contains(ext))
                    {
                        response.Message = $"Picture has an invalid extension: {MinimalPictureSize}, expected one of [ {string.Join(", ", ExtensionsForMimeType[picture.ContentType])}]!";
                        response.Successful = false;
                        response.StatusCode = StatusCodes.Status400BadRequest;
                    }
                }
            }

            return response;
        }
    }
}

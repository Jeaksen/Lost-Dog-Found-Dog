using Backend.Models.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Services.Security
{
    public class SecurityService : ISecurityService
    {
        private int MinimalPictureSize = 65;
        private List<string> AllowedPictureMimeTypes = new List<string>() { "image/png", "image/jpeg" };
        private Dictionary<string, List<string>> ExtensionsForMimeType = new Dictionary<string, List<string>>()
        {
            { "image/png", new List<string>() { "png"} },
            { "image/jpeg", new List<string>() { "jpg", "jpeg" } }
        };

        public ServiceResponse IsPictureValid(IFormFile picture)
        {
            var response = new ServiceResponse();
            if (picture.Length < MinimalPictureSize)
            {
                response.Message = $"Picture has size smaller than {MinimalPictureSize}!";
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

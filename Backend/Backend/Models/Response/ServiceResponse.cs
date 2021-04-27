using Microsoft.AspNetCore.Http;

namespace Backend.Models.Response
{
    public class ServiceResponse
    {
        public int StatusCode { get; set; } = StatusCodes.Status200OK;

        public bool Successful { get; set; } = true;

        public string Message { get; set; } = "No info provided";
    }

    public class ServiceResponse<TData> : ServiceResponse
    {
        public TData Data { get; set; }
    }

    public class ServiceResponse<TData, TMetadata> : ServiceResponse<TData>
    {
        public TMetadata Metadata { get; set; }
    }
}

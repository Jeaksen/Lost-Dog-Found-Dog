namespace Backend.Models.Response
{
    public class ControllerResponse
    {
        public bool Successful { get; set; } = true;

        public string Message { get; set; } = "No info provided";
    }

    public class ControllerResponse<TData> : ControllerResponse
    {
        public TData Data { get; set; }
    }

    public class ControllerResponse<TData, TMetadata> : ControllerResponse<TData>
    {
        public TMetadata Metadata { get; set; }
    }
}

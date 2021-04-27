namespace Backend.Models.Response
{
    public class RepositoryResponse
    {
        public bool Successful { get; set; } = true;

        public string Message { get; set; } = "No info provided";
    }

    public class RepositoryResponse<TData> : RepositoryResponse
    {
        public TData Data { get; set; }
    }

    public class RepositoryResponse<TData, TMetadata> : RepositoryResponse<TData>
    {
        public TMetadata Metadata { get; set; }
    }
}

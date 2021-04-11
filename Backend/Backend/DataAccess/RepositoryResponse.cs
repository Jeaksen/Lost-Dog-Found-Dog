using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DataAccess
{
    public class RepositoryResponse<T>
    {
        public T Data { get; set; }

        public bool Successful { get; set; } = true;

        public string Message { get; set; } = "No info provided";
    }
}

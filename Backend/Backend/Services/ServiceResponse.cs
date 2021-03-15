using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }

        public bool Successful { get; set; }

        public string Message { get; set; }

        public ServiceResponse(T data, bool successul = true, string message = null)
        {
            Data = data;
            Successful = successul;
            Message = message;
        }
    }
}

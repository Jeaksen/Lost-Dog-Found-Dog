using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }

        public bool Successful { get; set; } = true;

        public string Message { get; set; } = null;
    }
}

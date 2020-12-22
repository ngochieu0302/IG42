using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Utils
{
    public class BaseResponse<T> : JsonMessage
    {
        public T Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class LogDataItem :BaseSimple
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ReturnCode { get; set; }
        public string Content { get; set; }
    }
}

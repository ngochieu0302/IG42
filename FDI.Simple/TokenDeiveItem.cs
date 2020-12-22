using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class TokenDeiveItem:BaseSimple
    {
        public string Token { get; set; }
        public string Mobile { get; set; }
        public int? AreaId { get; set; }
        public string App { get; set; }
        public string UserId { get; set; }
    }
}

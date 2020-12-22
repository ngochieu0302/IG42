using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class CateAppItem:BaseSimple
    {
        public string img { get; set; }
        public string n { get; set; } // Name
        public int? PId { get; set; } // ID cha
        public int? c { get; set; } // countc
        public int? sort { get; set; } // Rút Tích lũy
        public IEnumerable<ProductItem> list { get; set; }
    }
}

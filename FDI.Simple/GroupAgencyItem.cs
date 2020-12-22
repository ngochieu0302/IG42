using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
     [Serializable]
    public class GroupAgencyItem : BaseSimple
    {
        public string Name { get; set; }
        public int? Level { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Description { get; set; }
        public bool? IsShow { get; set; }
    }
}

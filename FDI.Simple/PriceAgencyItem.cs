using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class PriceAgencyItem : BaseSimple
    {
        public string Name { get; set; }
        public int? Price { get; set; }
        public int? Sort { get; set; }
        public bool? IsShow { get; set; }
        public int? AgencyID { get; set; }
    }
}

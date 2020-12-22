using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class DNPacketItem : BaseSimple
    {
        public string Name { get; set; }
        public int? Sort { get; set; }
        public int? AgencyID { get; set; }

        public virtual IEnumerable<BedDeskItem> DN_Bed_Desk { get; set; }
        public virtual IEnumerable<ProductItem> Shop_Product { get; set; }
    }
}

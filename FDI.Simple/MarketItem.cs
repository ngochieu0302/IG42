using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
   public class MarketItem:BaseSimple
    {
        public string Name { get; set; }
        public int? AreaID { get; set; }
        public string Coordinates { get; set; }
        public int? WardsID { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public string Areaname { get; set; }
        public string Address { get; set; }
        public virtual AreaItem Area { get; set; }
        public IEnumerable<DNRequestWareHouseItem> ListDnRequestWareHouseItems { get; set; }
        public IEnumerable<DNRequestWareHouseActiveItem> ListDnRequestWareHouseGroupItems { get; set; }
        public virtual IEnumerable<DNAgencyItem> DnAgencyItems { get; set; }
        public virtual WardsItem Ward { get; set; }
    }

    public class ModelMarketItem : BaseModelSimple
    {
        public IEnumerable<MarketItem> ListItems { get; set; }
    }
}

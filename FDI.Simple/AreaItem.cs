using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class AreaItem:BaseSimple
    {
        public string Name { get; set; }
        public string Coordinates { get; set; }
        public int? AgencyID { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CityID { get; set; }
        public virtual DNAgencyItem DnAgencyItem { get; set; }
        public virtual IEnumerable<MarketItem> MarketItems { get; set; }
    }

    public class ModelAreaItem : BaseModelSimple
    {
        public IEnumerable<AreaItem> ListItems { get; set; }
    }

    public class AreaStaticItem : BaseSimple
    {
        public string Name { get; set; }
        public int? AgencyID { get; set; }
        public int? CityID { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalB { get; set; }
        public decimal? TotalC { get; set; }
    }
    public class ModelAreaStaticItem : BaseModelSimple
    {
        public IEnumerable<AreaStaticItem> ListItems { get; set; }
    }
}

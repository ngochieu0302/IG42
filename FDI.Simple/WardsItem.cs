using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class WardsItem:BaseSimple
    {
        public string Name { get; set; }
        public int? DistrictID { get; set; }
        public string Coordinates { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual IEnumerable<MarketItem> Markets { get; set; }
        public virtual DistrictItem DistrictItem { get; set; }
    }

    public class ModelWardsItem : BaseModelSimple
    {
        public IEnumerable<WardsItem> ListItems { get; set; }
    }
}

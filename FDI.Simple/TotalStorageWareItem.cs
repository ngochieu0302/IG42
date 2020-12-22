using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class TotalStorageWareItem:BaseSimple
    {
        public int? CateID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityOut { get; set; }
        public decimal? Today { get; set; }
        public int? Hour { get; set; }
        public string Catename { get; set; }
        public decimal? Cateprice { get; set; }
        public virtual IEnumerable<StorageProductItem> StorageProducts { get; set; }
    }

    public class ModelTotalStorageWareItem : BaseModelSimple
    {
        public IEnumerable<TotalStorageWareItem> ListItems { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple.Logistics;

namespace FDI.Simple.StorageWarehouse
{
    public class PurchaseOrderItem : BaseSimple
    {
        public int OrderCarID { get; set; }
        public int[] ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal Weight { get; set; }
        public string UserCreate { get; set; }
        public decimal CreateDate { get; set; }
        public string Note { get; set; }
    }

    public class PurchaseOrderModel : BaseModelSimple
    {
        public List<OrderCarProductDetailItem> ListItems { get; set; }

        public List<PurchaseOrderItem> Items { get; set; }
        public PurchaseOrderItem Item { get; set; }
        public OrderCarItem OrderCar { get; set; }
        
    }


}

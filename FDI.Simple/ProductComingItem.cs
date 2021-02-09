using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple.Supplier;

namespace FDI.Simple
{
    public class ProductComingItem:BaseSimple
    {
        public int? ProductID { get; set; }
        public string Productname { get; set; }
        public string Catename { get; set; }
        public string Ncc { get; set; }
        public int? SupplierAmountId { get; set; }
        public decimal? DateEx { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? QuantityOut { get; set; }

        public  ShopProductDetailItem Shop_Product_Detail { get; set; }
        public  SupplierAmountProductItem SupplierAmountProduct { get; set; }
    }

    public class ModelProductComingItem:BaseModelSimple
    {
        public IEnumerable<ProductComingItem> ListItems { get; set; }
    }
}

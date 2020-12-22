using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class FreightWarehouseItem:BaseSimple
    {
        public int? StorageProductID { get; set; }
        public int? ProductID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDelete { get; set; }
        public decimal? Date { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public StorageFreightWarehouseItem StorageFreightWarehouse { get; set; }
    }
    public class FreightWarehouseActiveItem : BaseSimple
    {
        public int? StorageProductID { get; set; }
        public int? ProductID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDelete { get; set; }
        public decimal? Date { get; set; }
        public string Barcode { get; set; }
        public decimal? DateE { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? ValueWeight { get; set; }
        public Guid? Idimport { get; set; }
        public StorageFreightWarehouseItem StorageFreightWarehouse { get; set; }
    }
    public class FreightWarehouseNewItem
    {
        public decimal? Quantity { get; set; }
        public string DateS { get; set; }
        public decimal? Price { get; set; }
        public int ProductID { get; set; }
        public string Key { get; set; }
        public decimal? TotalPrice { get; set; }
    }
    public class FreightWarehouseActiveNewItem
    {
        public decimal? Quantity { get; set; }
        public string DateS { get; set; }
        public string DateE { get; set; }
        public decimal? Price { get; set; }
        public int ProductID { get; set; }
        public string Key { get; set; }
        public decimal? TotalPrice { get; set; }
        public string BarCode { get; set; }
        public decimal? ValueWeight { get; set; }
        public Guid? Idimport { get; set; }
    }
}

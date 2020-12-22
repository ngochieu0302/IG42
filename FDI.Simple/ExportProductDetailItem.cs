using System;

namespace FDI.Simple
{
    public class ExportProductDetailItem : BaseSimple
    {
        public decimal? StorageProductId { get; set; }
        public int? ValueId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Date { get; set; }
        public bool? IsDelete { get; set; }
        public string Code { get; set; }
        public string ValueName { get; set; }
        public string ProductName { get; set; }
        public int? ProductID { get; set; }
    }
}

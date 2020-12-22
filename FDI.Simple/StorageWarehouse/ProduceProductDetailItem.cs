using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple.StorageWarehouse
{
    public class ProduceProductDetailItem
    {
        public int ProduceId { get; set; }
        public int ProductId { get; set; }
        public int ProductParentId { get; set; }
        public decimal Quantity { get; set; }
        public int? SizeId { get; set; }
        public string UnitName { get; set; }
        public decimal Weight { get; set; }
        public string ProductName { get; set; }
    }
}

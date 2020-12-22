using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.CORE;

namespace FDI.Simple.StorageWarehouse
{
    public class ProduceItem : BaseSimple
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityActive { get; set; }
        public decimal ToDayCode { get; set; }
        public ProduceStatus Status { get; set; }
        public  decimal DateProduce { get; set; }
    }

}

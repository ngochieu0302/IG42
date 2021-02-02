using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace FDI.Simple
{
    [Serializable]
    public class ProducComingsoonItem : BaseSimple
    {
        public int? ProductID { get; set; }
        public int? SupplierAmountId { get; set; }
        public decimal? DateEx { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? QuantityOut { get; set; }
    }
}

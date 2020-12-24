using System;
using System.Collections.Generic;

namespace FDI.Simple
{

    [Serializable]
    public class SaleCodeAppIG4Item : BaseSimple
    {
        public string Code { get; set; }
        public bool? IsUser { get; set; }
        public int? SaleId { get; set; }
        public decimal? DateUser { get; set; }
        public decimal? Percent { get; set; }
        public decimal? Price { get; set; }
    }
    public class ModelSaleCodeAppIG4Item : BaseModelSimple
    {
        public IEnumerable<SaleCodeAppIG4Item> ListItems { get; set; }
    }

}

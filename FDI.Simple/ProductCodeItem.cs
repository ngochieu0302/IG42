using System.Collections.Generic;

namespace FDI.Simple
{
    public class ProductCodeItem : BaseSimple
    {
        public string Code { get; set; }
        public int? Status { get; set; }
        public decimal? DateCreated { get; set; }
        public int? BiasProduceID { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? EndDate { get; set; }
        public string Note { get; set; }
    }

    public class ModelProductCodeItem : BaseModelSimple
    {
        public IEnumerable<ProductCodeItem> ListItems { get; set; } 
    }
}

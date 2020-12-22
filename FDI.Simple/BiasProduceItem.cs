using System.Collections.Generic;

namespace FDI.Simple
{
    public class BiasProduceItem : BaseSimple
    {
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? EndDate { get; set; }
        public int? OrderID { get; set; }
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<CostProductUserItem> LstCostProductUserItems { get; set; }
        public IEnumerable<CostProductItem> LstCostProducts { get; set; }
        public IEnumerable<SetupProductionItem> SetupProductionItems { get; set; }
    }
    public class ModelBiasProduceItem : BaseModelSimple
    {
        public IEnumerable<BiasProduceItem> ListItems { get; set; }
    }
}

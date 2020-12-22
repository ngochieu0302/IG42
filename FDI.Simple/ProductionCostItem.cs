using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class ProductionCostItem : BaseSimple
    {
        public string Name { get; set; }
        public int? AgencyID { get; set; }
        public double? Percent { get; set; }
        public decimal? TotalPercent { get; set; }
        public IEnumerable<CostProductUserItem> LstCostProductUserItems { get; set; }
        public IEnumerable<CostProductItem> LstCostProducts { get; set; }
    }
    public class ModelProductionCostItem : BaseModelSimple
    {
        public IEnumerable<ProductionCostItem> ListItems { get; set; }
    }
    public class CostProductUserItem : BaseSimple
    {
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string SetupProductName { get; set; }
        public string Key { get; set; }
        public int? SetupProductID { get; set; }
        public int? BiasProduceID { get; set; }
        public decimal? Percent { get; set; }
    }
    public class CostProductItem : BaseSimple
    {
        public string SetupProductName { get; set; }
        public int? SetupProductID { get; set; }
        public decimal? Percent { get; set; }
        public int? BiasProduceID { get; set; }
    }
    public class CostProductCostUserItem : BaseSimple
    {
        public string ProductCodeID { get; set; }
        public string SetupProductName { get; set; }
        public int? SetupProductID { get; set; }
        public string UserName { get; set; }        
        public int? Status { get; set; }
        public Guid? UserCreated { get; set; }
        public string UserCreatedName { get; set; }
        public decimal? DateCreated { get; set; }
        public string Note { get; set; }
    }
}

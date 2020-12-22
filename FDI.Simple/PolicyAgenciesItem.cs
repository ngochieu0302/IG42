using System;
using System.Collections.Generic;

namespace FDI.Simple
{
 
    public class PolicyAgenciesItem : BaseSimple
    {
        public int CategoryId { get; set; }
        public int LevelAgency { get; set; }
        public decimal Quantity { get; set; }
        public int Formula { get; set; }
        public decimal Profit { get; set; }
        public decimal PercentProfit { get; set; }
    }
    public class ModelPolicyAgenciesItem : PolicyAgenciesItem
    {
        public IEnumerable<CategoryItem> Categories { get; set; }
    }

    public class ModelCategoryPolicy:CategoryItem
    {
        public IEnumerable<PolicyAgenciesItem> Policies { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class CostTypeItem : BaseSimple
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Type { get; set; }

    }
    public class ModelCostType : BaseModelSimple
    {
        public IEnumerable<CostTypeItem> ListItem { get; set; }
    }
}

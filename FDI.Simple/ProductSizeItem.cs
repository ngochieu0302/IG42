using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class ProductSizeItem : BaseSimple
    {
        public string Name { get; set; }
        public double? Value { get; set; }
        public int? AgencyID { get; set; }
    }
    public class ModelProductSizeItem : BaseModelSimple
    {
        public IEnumerable<ProductSizeItem> ListItem { get; set; }
    }
}

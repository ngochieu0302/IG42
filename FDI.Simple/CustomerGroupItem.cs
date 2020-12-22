using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class CustomerGroupItem : BaseSimple
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Discount { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Level { get; set; }
        public decimal? TotalPrice { get; set; }
    }
    public class ModelCustomerGroupItem : BaseModelSimple
    {
        public IEnumerable<CustomerGroupItem> ListItem { get; set; }
    }
}

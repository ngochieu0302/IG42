using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class AddMinuteItem : BaseSimple
    {
        public string Name { get; set; }
        public int? Minute { get; set; }
        public int? Type { get; set; }
        public decimal? Price { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? DateCreated { get; set; }

        public virtual IEnumerable<OrderItem> Shop_Orders { get; set; }
    }
    public class ModelAddMinuteItem : BaseModelSimple
    {
        public IEnumerable<AddMinuteItem> ListItem { get; set; }
        
    }
}

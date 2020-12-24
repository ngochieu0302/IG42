using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class CustomerTypeAppIG4Item : BaseSimple
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Month { get; set; }
        public int? Type { get; set; }
        public int? Day { get; set; }
        public int? PictureId { get; set; }
        public decimal? Price { get; set; }
        public bool? Border { get; set; }
        public string Color { get; set; }
        public int? Sort { get; set; }
    }
    public class ModelCustomerTypeAppIG4Item : BaseModelSimple
    {
        public IEnumerable<CustomerTypeAppIG4Item> ListItem { get; set; }
    }
}
